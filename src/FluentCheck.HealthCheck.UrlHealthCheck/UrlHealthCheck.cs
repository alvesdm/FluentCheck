using System;
using System.Net;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace FluentCheck.HealthCheck.UrlHealthCheck
{
    public class UrlHealthCheck : HealthCheckRegisterBase, IUrlHealthCheck
    {
        private HttpStatusCode _expectedCode = HttpStatusCode.OK;
        private string _healthyMessage = "The URL seems to be OK.";
        private string _unhealthyMessage = "The URL seems to be down.";
        private static HttpClient _client;
        private string _user;
        private string _password;
        private string _address;
        private TimeSpan _timeout;

        public override async Task<HealthCheckResult> Check(bool isLocalRequest)
        {
            var ret = false;
            var reason = string.Empty;
            try
            {
                if (_client == null)
                {
                    if (!string.IsNullOrWhiteSpace(_user) && !string.IsNullOrWhiteSpace(_password))
                        _client = new HttpClient(new HttpClientHandler
                        {
                            Credentials = new NetworkCredential(_user, _password)
                        });
                    else
                        _client = new HttpClient();
                }

                CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
                if (_timeout.TotalMilliseconds > 0)
                    cancellationTokenSource.CancelAfter(_timeout);

                using (var async = await _client.GetAsync(_address, cancellationTokenSource.Token))
                {
                    if (async.StatusCode == _expectedCode)
                        ret = true;
                    else
                        SetReason(isLocalRequest, string.Format("{0} - {1}", async.StatusCode, async.ReasonPhrase), ref reason);
                }
            }
            catch (Exception ex)
            {
                SetReason(isLocalRequest, AggregateExceptions(ex), ref reason);
            }
            return ret ? Healthy(_healthyMessage) : Unhealthy(_unhealthyMessage, reason);
        }

        public IUrlHealthCheck WithAddress(string address)
        {
            _address = address;
            return this;
        }

        public IUrlHealthCheck WithCredentials(string user, string password)
        {
            _user = user;
            _password = password;
            return this;
        }

        public IUrlHealthCheck WithExpectedResult(HttpStatusCode code)
        {
            _expectedCode = code;
            return this;
        }

        public IUrlHealthCheck WithMessages(string healthyMessage, string unhealthyMessage)
        {
            _unhealthyMessage = unhealthyMessage;
            _healthyMessage = healthyMessage;
            return this;
        }

        public IUrlHealthCheck WithTimeout(TimeSpan timeout)
        {
            _timeout = timeout;
            return this;
        }

        public IUrlHealthCheck WithTimeout(int seconds)
        {
            _timeout = TimeSpan.FromSeconds(seconds);
            return this;
        }
    }
}