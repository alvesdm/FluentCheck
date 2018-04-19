using System.Threading.Tasks;

namespace FluentCheck.HealthCheck
{
    public interface IHealthCheckRegister
    {
        string Name { get; set; }

        Task<HealthCheckResult> Check(bool isLocalRequest);
    }
}