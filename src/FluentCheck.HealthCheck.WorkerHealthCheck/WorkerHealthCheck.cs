using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace FluentCheck.HealthCheck.WorkerHealthCheck
{
    public class WorkerHealthCheck : HealthCheckRegisterBase, IWorkerHealthCheck
    {
        private string _healthyMessage = "The worker seems to be OK.";
        private string _unhealthyMessage = "The worker seems to be down.";
        private string _address = "/worker/ping";
        private TimeSpan _timeout = TimeSpan.FromSeconds(5);
        private DateTime _lastPinged;

        public override async Task<HealthCheckResult> Check(bool isLocalRequest)
        {
            var reason = _lastPinged.Equals(DateTime.MinValue) ? "Worker haven't pinged back yet." : $"Last ping back was at {_lastPinged:G}.";

            var ret = !(DateTime.Now.AddSeconds(_timeout.TotalSeconds * -1) > _lastPinged);

            var result = ret ? Healthy(_healthyMessage) : Unhealthy(_unhealthyMessage, reason);

            return await Task.FromResult(result);
        }

        public IWorkerHealthCheck WithPingAddress(string address)
        {
            _address = $"/{address}".Replace("//", "/");
            return this;
        }

        public IWorkerHealthCheck WithMaxLastPingTime(TimeSpan timeout)
        {
            _timeout = timeout;
            return this;
        }

        public IWorkerHealthCheck WithMaxLastPingTime(int seconds)
        {
            _timeout = TimeSpan.FromSeconds(seconds);
            return this;
        }

        public Action<HttpContext> Response => (httpContext) =>
        {
            _lastPinged = DateTime.Now;
        };

        public string Url => _address;
    }
}