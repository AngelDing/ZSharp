using System;
using System.Linq;
using System.Net;

namespace ZSharp.Framework.Web.Throttle
{
    public class ThrottleProcesser : IThrottleProcesser
    {        
        private ThrottleProcessResult processResult;       

        /// <summary>
        /// Initializes a new instance of the <see cref="ThrottleProcesser"/> class.
        /// By default, the <see cref="QuotaExceededResponseCode"/> property 
        /// is set to 429 (Too Many Requests).
        /// </summary>      
        public ThrottleProcesser(
            ThrottlePolicy policy,
            IIpAddressParser ipAddressParser,
            IPolicyRepository policyRepo = null,
            IThrottleRepository throttleRepo = null)
        {
            Logger = new DefaultThrottleLogger();
            QuotaExceededResponseCode = (HttpStatusCode)429;
            processResult = new ThrottleProcessResult { IsPass = true };

            ThrottleRepo = throttleRepo;
            if (ThrottleRepo == null)
            {
                ThrottleRepo = new WebCacheThrottleRepository();
            }
            ThrottlingCore = new ThrottlingCore(ipAddressParser);
            ThrottlingCore.ThrottleRepo = ThrottleRepo;

            PolicyRepo = policyRepo;
            if (PolicyRepo == null)
            {
                PolicyRepo = new WebCachePolicyRepository();
            }
            Policy = policy;
            PolicyRepo.Save(ThrottleManager.GetPolicyKey(), policy);
        }

        #region Property

        /// <summary>
        ///  Gets or sets a repository used to access throttling rate limits policy.
        /// </summary>
        internal IPolicyRepository PolicyRepo { get; set; }       

        /// <summary>
        /// Gets or sets the throttle metrics storage
        /// </summary>
        internal IThrottleRepository ThrottleRepo { get; set; }

        internal RequestIdentity Identity { get; set; }

        /// <summary>
        /// Gets or sets the throttling rate limits policy
        /// </summary>
        public ThrottlePolicy Policy { get; set; }

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

        protected ThrottlingCore ThrottlingCore { get; private set; }

        #endregion

        public IPAddress GetClientIp(object request)
        {
            return ThrottlingCore.GetClientIp(request);
        }

        public ThrottleProcessResult Process(RequestIdentity identity, IEnableThrottlingAttribute attrPolicy = null)
        {
            Identity = identity;
            Policy = PolicyRepo.FirstOrDefault(ThrottleManager.GetPolicyKey());
            if (Policy != null)
            {
                Checking(attrPolicy);
            }
            return processResult;
        }

        private void Checking(IEnableThrottlingAttribute attrPolicy)
        {
            ThrottlingCore.ThrottleRepo = ThrottleRepo;
            ThrottlingCore.Policy = Policy;

            if (!Policy.IpThrottling && !Policy.ClientThrottling && !Policy.EndpointThrottling)
            {
                return;
            }
            if (ThrottlingCore.IsWhitelisted(Identity))
            {
                return;
            }
            TimeSpan timeSpan = TimeSpan.FromSeconds(1);
            // get default rates
            var defRates = ThrottlingCore.RatesWithDefaults(Policy.Rates.ToList());
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
                timeSpan = ThrottlingCore.GetTimeSpanFromPeriod(rateLimitPeriod);
                if (attrPolicy != null)
                {
                    // apply EnableThrottlingAttribute policy
                    var attrLimit = attrPolicy.GetLimit(rateLimitPeriod);
                    if (attrLimit > 0)
                    {
                        rateLimit = attrLimit;
                    }
                }
                // apply global rules
                ThrottlingCore.ApplyRules(Identity, timeSpan, rateLimitPeriod, ref rateLimit);
                if (rateLimit == 0)
                {
                    continue;
                }
                // increment counter
                string requestId;
                var throttleCounter = ThrottlingCore.ProcessRequest(Identity, timeSpan, rateLimitPeriod, out requestId);
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
                        var logEntry = ThrottlingCore.ComputeLogEntry(requestId, Identity, throttleCounter,
                            rateLimitPeriod.ToString(), rateLimit);
                        Logger.Log(logEntry);
                    }
                    var retryAfter = ThrottlingCore.RetryAfterFrom(throttleCounter.Timestamp, rateLimitPeriod);
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
    }
}
