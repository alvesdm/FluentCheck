using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace FluentCheck.HealthCheck.WorkerHealthCheck
{
    public class WorkerClient
    {
        private static HttpClient _client;

        public async Task<bool> PingBack(string url)
        {
            bool ret;
            try
            {
                if (_client == null)
                {
                    _client = new HttpClient();
                }

                using (var async = await _client.GetAsync(url))
                {
                    ret = async.StatusCode == HttpStatusCode.OK;
                }
            }
            catch
            {
                ret = false;
            }

            return await Task.FromResult(ret);
        }
    }
}