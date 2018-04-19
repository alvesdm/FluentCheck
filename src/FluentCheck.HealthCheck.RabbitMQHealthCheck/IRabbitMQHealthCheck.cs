namespace FluentCheck.HealthCheck.RabbitMQHealthCheck
{
    public interface IRabbitMQHealthCheck : IHealthCheckRegister
    {
        IRabbitMQHealthCheck WithHost(string host, int port = 15672);

        IRabbitMQHealthCheck WithVhost(string vHost);

        IRabbitMQHealthCheck WithQueue(string queueName);

        IRabbitMQHealthCheck WithCredentials(string user, string password);

        IRabbitMQHealthCheck WithHttps();
    }
}