using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using MShop.Messaging.Configuration;
using RabbitMQ.Client;

namespace MShop.Application.Health
{
    public class HealthMessagingBroker : IHealthCheck
    {
        private readonly RabbitMQConfiguration _rabbitmqConfiguration;

        public HealthMessagingBroker(IOptions<RabbitMQConfiguration> rabbitmqConfiguration)
        {
            _rabbitmqConfiguration = rabbitmqConfiguration.Value;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            try
            {
                var factory = new ConnectionFactory
                {
                    HostName = _rabbitmqConfiguration.HostName,
                    UserName = _rabbitmqConfiguration.UserName,
                    Password = _rabbitmqConfiguration.Password,
                    Port = _rabbitmqConfiguration.Port,
                    VirtualHost = _rabbitmqConfiguration.Vhost
                };

                var conexao = factory.CreateConnection();

                if (conexao.IsOpen)
                    return HealthCheckResult.Healthy();
                else 
                    return HealthCheckResult.Unhealthy();

            }
            catch (Exception ex)
            {
                return await Task.FromResult(new HealthCheckResult(
                    status : HealthStatus.Unhealthy,
                    description : ex.Message
                    ));
            }
        }
    }
}
