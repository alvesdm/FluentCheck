using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace FluentCheck.HealthCheck.RabbitMQHealthCheck
{
    public class RabbitMQHealthCheck : HealthCheckRegisterBase, IRabbitMQHealthCheck
    {
        private string _host = "localhost";
        private int _port = 15672;
        private string _vHost = "/";
        private string _user = "guest";
        private string _password = "guest";
        private string _queueName = string.Empty;
        private static HttpClient _client;
        private bool _isHttps;

        public override async Task<HealthCheckResult> Check(bool isLocalRequest)
        {
            var ret = false;
            var reason = string.Empty;
            try
            {
                if (_client == null)
                    _client = new HttpClient(new HttpClientHandler
                    {
                        Credentials = new NetworkCredential(_user, _password)
                    });
                var requestUri = string.Format(
                    "{0}://{1}:{2}/api/queues/{3}/{4}"
                    , _isHttps ? "https" : "http"
                    , _host
                    , _port
                    , Uri.EscapeDataString(_vHost)
                    , Uri.EscapeDataString(_queueName));

                using (var async = await _client.GetAsync(requestUri))
                {
                    if (async.StatusCode == HttpStatusCode.OK)
                        ret = true;
                    else
                        SetReason(isLocalRequest, string.Format("{0} - {1}", async.StatusCode, async.ReasonPhrase), ref reason);
                }
            }
            catch (Exception ex)
            {
                SetReason(isLocalRequest, AggregateExceptions(ex), ref reason);
            }
            return ret ? Healthy("RabbitMQ seems to be OK.") : Unhealthy("RabbitMQ looks not happy.", reason);
        }

        public IRabbitMQHealthCheck WithHost(string host, int port)
        {
            _host = host;
            _port = port;
            return this;
        }

        public IRabbitMQHealthCheck WithCredentials(string user, string password)
        {
            _user = user;
            _password = password;
            return this;
        }

        public IRabbitMQHealthCheck WithQueue(string queueName)
        {
            _queueName = queueName;
            return this;
        }

        public IRabbitMQHealthCheck WithVhost(string vHost)
        {
            _vHost = vHost;
            return this;
        }

        public IRabbitMQHealthCheck WithHttps()
        {
            _isHttps = true;
            return this;
        }
    }
}
