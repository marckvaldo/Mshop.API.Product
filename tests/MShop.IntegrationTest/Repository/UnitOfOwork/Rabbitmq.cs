using MShop.Core.Message.DomainEvent;

namespace MShop.IntegrationTests.Repository.UnitOfOwork
{
    public class RabbitmqFaker : IMessageProducer
    {
        public Task SendMessageAsync<T>(T message)
        {
            return Task.CompletedTask;
        }
    }
}
