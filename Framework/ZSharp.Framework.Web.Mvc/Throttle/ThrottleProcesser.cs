using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ZSharp.Framework.Web.Throttle;

namespace ZSharp.Framework.Web.Mvc.Throttle
{
    public class ThrottleCheckResult
    {
        public bool IsPass { get; set; }
        public string Message { get; set; }
        public int StatusCode { get; set; }
        public KeyValuePair<string, string> HeadEntry { get; set; }
    }
    public class ThrottleProcesser : IThrottleProcesser
    {
        private ThrottlingCore core;
        private IPolicyRepository policyRepository;
        private ThrottlePolicy policy;

        /// <summary>
        /// Initializes a new instance of the <see cref="ThrottlingHandler"/> class. 
        /// By default, the <see cref="QuotaExceededResponseCode"/> property 
        /// is set to 429 (Too Many Requests).
        /// </summary>
        public ThrottleProcesser()
        {
            QuotaExceededResponseCode = (HttpStatusCode)429;
            Repository = new WebCacheThrottleRepository();
            core = new ThrottlingCore();
            Logger = new DefaultThrottleLogger();
            
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ThrottlingHandler"/> class.
        /// Persists the policy object in cache using <see cref="IPolicyRepository"/> implementation.
        /// The policy object can be updated by <see cref="ThrottleManager"/> at runtime. 
        /// </summary>
        /// <param name="policy">
        /// The policy.
        /// </param>
        /// <param name="policyRepository">
        /// The policy repository.
        /// </param>
        /// <param name="repository">
        /// The repository.
        /// </param>
        /// <param name="logger">
        /// The logger.
        /// </param>
        public ThrottleProcesser(ThrottlePolicy policy, IPolicyRepository policyRepository, IThrottleRepository repository, IThrottleLogger logger)
        {
            core = new ThrottlingCore();
            core.Repository = repository;
            Repository = repository;
            Logger = logger;

            QuotaExceededResponseCode = (HttpStatusCode)429;

            this.policy = policy;
            this.policyRepository = policyRepository;

            if (policyRepository != null)
            {
                policyRepository.Save(ThrottleManager.GetPolicyKey(), policy);
            }
        }

        /// <summary>
        ///  Gets or sets the throttling rate limits policy repository
        /// </summary>
        public IPolicyRepository PolicyRepository
        {
            get { return policyRepository; }
            set { policyRepository = value; }
        }

        /// <summary>
        /// Gets or sets the throttling rate limits policy
        /// </summary>
        public ThrottlePolicy Policy
        {
            get { return policy; }
            set { policy = value; }
        }

        /// <summary>
        /// Gets or sets the throttle metrics storage
        /// </summary>
        public IThrottleRepository Repository { get; set; }

        /// <summary>
        /// Gets or sets an instance of <see cref="IThrottleLogger"/> that logs traffic and blocked requests
        /// </summary>
        public IThrottleLogger Logger { get; set; }

        /// <summary>
        /// Gets or sets a value that will be used as a formatter for the QuotaExceeded response message.
        /// If none specified the default will be: 
        /// API calls quota exceeded! maximum admitted {0} per {1}
        /// </summary>
        public string QuotaExceededMessage { get; set; }

        /// <summary>
        /// Gets or sets the value to return as the HTTP status 
        /// code when a request is rejected because of the
        /// throttling policy. The default value is 429 (Too Many Requests).
        /// </summary>
        public HttpStatusCode QuotaExceededResponseCode { get; set; }

        private bool ApplyThrottling(ActionExecutingContext filterContext, out EnableThrottlingAttribute attr)
        {
            var applyThrottling = true;
            attr = null;

            if (filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(EnableThrottlingAttribute), true))
            {
                attr = (EnableThrottlingAttribute)filterContext.ActionDescriptor.ControllerDescriptor.GetCustomAttributes(typeof(EnableThrottlingAttribute), true).First();
                applyThrottling = true;
            }

            if (filterContext.ActionDescriptor.IsDefined(typeof(EnableThrottlingAttribute), true))
            {
                attr = (EnableThrottlingAttribute)filterContext.ActionDescriptor.GetCustomAttributes(typeof(EnableThrottlingAttribute), true).First();
                applyThrottling = true;
            }

            //explicit disabled
            if (filterContext.ActionDescriptor.IsDefined(typeof(DisableThrottingAttribute), true))
            {
                applyThrottling = false;
            }

            return applyThrottling;
        }
        public ThrottleCheckResult ThrottleChecking(ActionExecutingContext context)
        {
           
            HttpRequestBase request = context.HttpContext.Request;
            EnableThrottlingAttribute attrPolicy = null;
            ThrottleCheckResult result = new ThrottleCheckResult() {  IsPass = true};
            RequestIdentity identity = null;
            // get policy from repo
            if (policyRepository != null)
            {
                policy = policyRepository.FirstOrDefault(ThrottleManager.GetPolicyKey());
            }
             bool isDisabled = ApplyThrottling(context, out attrPolicy);
             if (policy == null || !isDisabled || (!policy.IpThrottling && !policy.ClientThrottling && !policy.EndpointThrottling))
            {
                
                return result;
            }

            core.Repository = Repository;
            core.Policy = policy;
            identity = SetIndentity(request, attrPolicy);

            if (core.IsWhitelisted(identity))
            {
                
                return result;
            }

            TimeSpan timeSpan = TimeSpan.FromSeconds(1);

            // get default rates
            var defRates = core.RatesWithDefaults(Policy.Rates.ToList());
            if (Policy.StackBlockedRequests)
            {
                // all requests including the rejected ones will stack in this order: week, day, hour, min, sec
                // if a client hits the hour limit then the minutes and seconds counters will expire and will eventually get erased from cache
                defRates.Reverse();
            }

            // apply policy
            foreach (var rate in defRates)
            {
                var rateLimitPeriod = rate.Key;
                var rateLimit = rate.Value;

                timeSpan = core.GetTimeSpanFromPeriod(rateLimitPeriod);

                // apply global rules
                core.ApplyRules(identity, timeSpan, rateLimitPeriod, ref rateLimit);
                // apply attrPolicy rules
                if (attrPolicy != null)
                {
                    var attrLimit = attrPolicy.GetLimit(rateLimitPeriod);
                    if (attrLimit > 0)
                    {
                        rateLimit = attrLimit;
                    }
                }
                if (rateLimit > 0)
                {
                    // increment counter
                    string requestId;
                    var throttleCounter = core.ProcessRequest(identity, timeSpan, rateLimitPeriod, out requestId);

                    // check if key expired
                    if (throttleCounter.Timestamp + timeSpan < DateTime.UtcNow)
                    {
                        continue;
                    }

                    // check if limit is reached
                    if (throttleCounter.TotalRequests > rateLimit)
                    {
                        // log blocked request
                        if (Logger != null)
                        {
                            Logger.Log(core.ComputeLogEntry(requestId, identity, throttleCounter, rateLimitPeriod.ToString(), rateLimit, request));
                        }

                        var message = !string.IsNullOrEmpty(this.QuotaExceededMessage)
                            ? this.QuotaExceededMessage
                            : "API calls quota exceeded! maximum admitted {0} per {1}.";
                        // break execution
                      
                        result.Message = string.Format(message, rateLimit, rateLimitPeriod);
                        result.HeadEntry = new KeyValuePair<string, string>("Retry-After", core.RetryAfterFrom(throttleCounter.Timestamp, rateLimitPeriod));
                        result.StatusCode = (int)QuotaExceededResponseCode;
                        result.IsPass = false;
                        return result;
                    }
                }
            }
           
            // no throttling required
            return result;
        }

        protected IPAddress GetClientIp(HttpRequestBase request)
        {
            return core.GetClientIp(request);
        }

        
        protected virtual RequestIdentity SetIndentity(HttpRequestBase request,EnableThrottlingAttribute attr)
        {
            var entry = new RequestIdentity();
            entry.ClientIp = GetClientIp(request).ToString();
            entry.ClientKey = request.IsAuthenticated ? "auth" : "anon";
            if (attr == null)
            {
                entry.Endpoint = request.Url.AbsolutePath.ToLowerInvariant();
                return entry;
            }
            var rd = request.RequestContext.RouteData;
            string currentAction = rd.GetRequiredString("action");
            string currentController = rd.GetRequiredString("controller");

            switch (Policy.EndpointType)
            {
                case EndpointThrottlingType.AbsolutePath:
                    entry.Endpoint = request.Url.AbsolutePath;
                    break;
                case EndpointThrottlingType.PathAndQuery:
                    entry.Endpoint = request.Url.PathAndQuery;
                    break;
                case EndpointThrottlingType.ControllerAndAction:
                    entry.Endpoint = currentController + "/" + currentAction;
                    break;
                case EndpointThrottlingType.Controller:
                    entry.Endpoint = currentController;
                    break;
                default:
                    break;
            }

            //case insensitive routes
            entry.Endpoint = entry.Endpoint.ToLowerInvariant();

            return entry;
        }
        protected virtual string ComputeThrottleKey(RequestIdentity requestIdentity, RateLimitPeriod period)
        {
            return core.ComputeThrottleKey(requestIdentity, period);
        }

       
    }
    
}