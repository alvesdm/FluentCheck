using System;
using System.Collections.Generic;

namespace FluentCheck.HealthCheck
{
    public class HealthCheckConfiguration : IHealthCheckConfiguration
    {
        private readonly IServiceProvider _serviceProvider;

        public string Endpoint { get; private set; }
        public IList<IHealthCheckRegister> HealthChecks { get; }

        public HealthCheckConfiguration(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            Endpoint = "/health";
            HealthChecks = new List<IHealthCheckRegister>();
        }

        public IHealthCheckConfiguration Register<T>(string name, Func<T, T> healthCheck) 
            where T : IHealthCheckRegister
        {
            var service = (T)_serviceProvider.GetService(typeof(T));
            service.Name = name;
            healthCheck(service);
            HealthChecks.Add(service);
            return this;
        }

        public IHealthCheckConfiguration WithEndpoint(string uri)
        {
            Endpoint = $"/{uri}".Replace("//", "/");
            return this;
        }
    }
}