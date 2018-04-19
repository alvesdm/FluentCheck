using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace FluentCheck.HealthCheck
{
    public static class HealthCheckApplicationBuilder
    {
        private static bool IsLocalRequest(HttpContext context)
        {
            return context.Connection.RemoteIpAddress.Equals(context.Connection.LocalIpAddress) || IPAddress.IsLoopback(context.Connection.RemoteIpAddress);
        }

        public static IApplicationBuilder UseHealthCheck(
            this IApplicationBuilder app,
            IServiceProvider serviceProvider,
            Func<IHealthCheckConfiguration, IHealthCheckConfiguration> config)
        {
            var c = (IHealthCheckConfiguration) serviceProvider.GetService(typeof(IHealthCheckConfiguration));
            config(c);

            app.MapWhen(
                context => context.Request.Path.Value.EndsWith(c.Endpoint),
                builder =>
                {
                    builder.Run(async httpContext =>
                    {
                        var checkTasks = c.HealthChecks.Select(async hc => await hc.Check(IsLocalRequest(httpContext)));
                        var checkResults = (await Task.WhenAll(checkTasks)).ToList();
                        var responseContent = new
                        {
                            isHealthy = checkResults.All(m => m.IsHealthy),
                            checks = checkResults.Select(f => new
                            {
                                name = f.Name,
                                result = f.Result,
                                isHealthy = f.IsHealthy
                            })
                        };

                        httpContext.Response.StatusCode =
                            responseContent.isHealthy
                                ? StatusCodes.Status200OK
                                : StatusCodes.Status503ServiceUnavailable;

                        await httpContext.Response.WriteAsync(JsonConvert.SerializeObject(responseContent));
                    });
                });

            return app;
        }
    }
}
