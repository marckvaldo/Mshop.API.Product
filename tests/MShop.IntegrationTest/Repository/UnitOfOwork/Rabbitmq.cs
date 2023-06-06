using MShop.Business.Interface.Event;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
