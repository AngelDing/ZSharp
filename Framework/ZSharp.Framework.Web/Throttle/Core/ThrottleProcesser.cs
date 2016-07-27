using System;
using System.Linq;
using System.Net;

namespace ZSharp.Framework.Web.Throttle
{
    public abstract class ThrottleProcesser
    {
        private readonly ThrottlingCore core;
        private IPolicyRepository policyRepository;
        private ThrottlePolicy policy;
        private ThrottleProcessResult processResult;

        public ThrottleProcesser()
        {
            processResult = new ThrottleProcessResult { IsPass = true };
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ThrottleProcesser"/> class.
        /// By default, the <see cref="QuotaExceededResponseCode"/> property 
        /// is set to 429 (Too Many Requests).
        /// </summary>
        public ThrottleProcesser(IIpAddressParser ipAddressParser) : this()
        {
            QuotaExceededResponseCode = (HttpStatusCode)429;
            Repository = new ThrottleWebCacheRepository();
            core = new ThrottlingCore(ipAddressParser);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ThrottleProcesser"/> class.
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
        /// <param name="ipAddressParser">
        /// The ip address provider
        /// </param>
        public ThrottleProcesser(
            ThrottlePolicy policy,
            IPolicyRepository policyRepository,
            IThrottleRepository repository,
            IThrottleLogger logger,
            IIpAddressParser ipAddressParser) : this()
        {
            core = new ThrottlingCore(ipAddressParser);
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
        ///  Gets or sets a repository used to access throttling rate limits policy.
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
        /// Gets or sets an instance of <see cref="IThrottleLogger"/> that will log blocked requests
        /// </summary>
        public IThrottleLogger Logger { get; set; }

        /// <summary>
        /// Gets or sets a value that will be used as a formatter for the QuotaExceeded response message.
        /// If none specified the default will be: 
        /// API calls quota exceeded! maximum admitted {0} per {1}
        /// </summary>
        public string QuotaExceededMessage { get; set; }

        /// <summary>
        /// Gets or sets a value that will be used as a formatter for the QuotaExceeded response message.
        /// If none specified the default will be: 
        /// API calls quota exceeded! maximum admitted {0} per {1}
        /// </summary>
        public Func<long, RateLimitPeriod, object> QuotaExceededContent { get; set; }

        /// <summary>
        /// Gets or sets the value to return as the HTTP status 
        /// code when a request is rejected because of the
        /// throttling policy. The default value is 429 (Too Many Requests).
        /// </summary>
        public HttpStatusCode QuotaExceededResponseCode { get; set; }

        public ThrottleProcessResult Process(object actionContext)
        {           
            IEnableThrottlingAttribute attrPolicy = null;
            var applyThrottling = ApplyThrottling(actionContext, out attrPolicy);
            // get policy from repo
            if (policyRepository != null)
            {
                policy = policyRepository.FirstOrDefault(ThrottleManager.GetPolicyKey());
            }
            if (Policy != null && applyThrottling)
            {
                var request = GetRequest(actionContext);
                Checking(request, attrPolicy);
            }
            return processResult;
        }

        private void Checking(object request, IEnableThrottlingAttribute attrPolicy)
        {
            core.Repository = Repository;
            core.Policy = Policy;
            var identity = SetIndentity(request);
            if (core.IsWhitelisted(identity))
            {
                return;
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
                // apply EnableThrottlingAttribute policy
                var attrLimit = attrPolicy.GetLimit(rateLimitPeriod);
                if (attrLimit > 0)
                {
                    rateLimit = attrLimit;
                }
                // apply global rules
                core.ApplyRules(identity, timeSpan, rateLimitPeriod, ref rateLimit);
                if (rateLimit == 0)
                {
                    continue;
                }
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
                        var logEntry = core.ComputeLogEntry(requestId, identity, throttleCounter,
                            rateLimitPeriod.ToString(), rateLimit, request);
                        Logger.Log(logEntry);
                    }
                    var retryAfter = core.RetryAfterFrom(throttleCounter.Timestamp, rateLimitPeriod);
                    ProcessQuotaExceeded(rateLimitPeriod, rateLimit, retryAfter);
                    return;
                }
            }
        }

        private void ProcessQuotaExceeded(RateLimitPeriod rateLimitPeriod, long rateLimit, string retryAfter)
        {            
            var message = !string.IsNullOrEmpty(QuotaExceededMessage) 
                ? QuotaExceededMessage
                : "API calls quota exceeded! maximum admitted {0} per {1}.";

            var content = QuotaExceededContent != null
                ? QuotaExceededContent(rateLimit, rateLimitPeriod)
                : string.Format(message, rateLimit, rateLimitPeriod);

            processResult.IsPass = false;
            processResult.Message = message;
            processResult.Content = content;
            processResult.RetryAfter = retryAfter;
            processResult.StatusCode = (int)QuotaExceededResponseCode;
        }

        protected abstract object GetRequest(object actionContext);

        protected abstract RequestIdentity SetIndentity(object request);

        protected virtual string ComputeThrottleKey(RequestIdentity requestIdentity, RateLimitPeriod period)
        {
            return core.ComputeThrottleKey(requestIdentity, period);
        }

        protected IPAddress GetClientIp(object request)
        {
            return core.GetClientIp(request);
        }

        protected abstract bool ApplyThrottling(object filterContext, out IEnableThrottlingAttribute attr);
    }
}
