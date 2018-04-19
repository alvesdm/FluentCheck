using System;
using System.Net;

namespace FluentCheck.HealthCheck.UrlHealthCheck
{
    public interface IUrlHealthCheck : IHealthCheckRegister
    {
        IUrlHealthCheck WithAddress(string address);

        IUrlHealthCheck WithExpectedResult(HttpStatusCode code);

        IUrlHealthCheck WithTimeout(TimeSpan timeout);

        IUrlHealthCheck WithTimeout(int seconds);

        IUrlHealthCheck WithCredentials(string user, string password);

        IUrlHealthCheck WithMessages(string healthyMessage, string unhealthyMessage);
    }
}
