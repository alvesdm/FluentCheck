namespace FluentCheck.HealthCheck
{
    public class HealthCheckResult
    {
        public string Name { get; set; }

        public bool IsHealthy { get; set; }

        public string Result { get; set; }
    }
}