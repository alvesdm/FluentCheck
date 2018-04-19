using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentCheck.HealthCheck.Utils;

namespace FluentCheck.HealthCheck
{
    public abstract class HealthCheckRegisterBase
    {
        public string Name { get; set; }

        public abstract Task<HealthCheckResult> Check(bool isLocalRequest);

        public virtual HealthCheckResult Healthy(string result)
        {
            return SetHealthState(true, result);
        }

        public virtual HealthCheckResult Unhealthy(string result, string reason = "")
        {
            return SetHealthState(false, !string.IsNullOrWhiteSpace(reason) ? string.Format("{0} Reason: {1}", result, reason) : result);
        }

        private HealthCheckResult SetHealthState(bool isHealthy, string result)
        {
            return new HealthCheckResult
            {
                Name = Name,
                IsHealthy = isHealthy,
                Result = result
            };
        }

        protected void SetReason(bool isLocalRequest, string reasonPhrase, ref string reason)
        {
            if (!isLocalRequest)
                return;

            reason = reasonPhrase;
        }

        protected string AggregateExceptions(Exception ex)
        {
            return ExceptionUtils.GetExceptions(ex)
                .Select((m, i) => $"#{i} - {m}")
                .Reverse()
                .Aggregate((x, y) => x + "; " + y);
        }
    }
}