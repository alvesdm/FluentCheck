using System;

namespace FluentCheck.HealthCheck.WorkerHealthCheck
{
    public interface IWorkerHealthCheck : IHealthCheckRegister, IRegisterRequest
    {
        /// <summary>
        /// The endpoint address to be registered so that worker can invoke.
        /// It will start with the value passed to .WithEndpoint("/healthcheck")
        /// i.e.: .WithEndpoint("/healthcheck") AND .WithPingAddress("/worker/ping") would register: "/healthcheck/worker/ping"
        /// Default: /worker/ping
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        IWorkerHealthCheck WithPingAddress(string address);

        /// <summary>
        /// Max waiting time before consider worker down
        /// Default: 5seconds
        /// </summary>
        /// <param name="timeout"></param>
        /// <returns></returns>
        IWorkerHealthCheck WithMaxLastPingTime(TimeSpan timeout);

        /// <summary>
        /// Max waiting time before consider worker down
        /// Default: 5seconds
        /// </summary>
        /// <param name="seconds"></param>
        /// <returns></returns>
        IWorkerHealthCheck WithMaxLastPingTime(int seconds);
    }
}
