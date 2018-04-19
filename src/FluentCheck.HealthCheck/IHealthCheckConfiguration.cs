using System;
using System.Collections.Generic;

namespace FluentCheck.HealthCheck
{
    public interface IHealthCheckConfiguration
    {
        string Endpoint { get; }

        IList<IHealthCheckRegister> HealthChecks { get; }

        IHealthCheckConfiguration Register<T>(string name, Func<T, T> healthCheck) 
            where T : IHealthCheckRegister;

        IHealthCheckConfiguration WithEndpoint(string uri);
    }
}