using Bogus;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MShop.Core.DomainObject;
using MShop.Messaging.Configuration;
using MShop.Repository.Context;
using System.Text;
using System.Text.Json;

namespace MShop.EndToEndTest.Common
{
    public abstract class BaseFixture
    {
        protected readonly Faker faker;
        protected static readonly Faker fakerStatic = new Faker("pt_BR");
        protected APIClient apiClient;
        protected CustomWebApplicationFactory<Program> webApp;
        protected HttpClient httpClient;
        private readonly string _connectionString;
        private readonly IConfiguration _configuration;
        private string _nameQueue = "history.v1.product";
        //private  string _routeKey = "product.#";
        private ServiceRabbitMQ _setupRabbitMQ;

        protected BaseFixture()
        {
            faker = new Faker("pt_BR");

            webApp = new CustomWebApplicationFactory<Program>();
            httpClient = webApp.CreateClient();
            apiClient = new APIClient(httpClient);

            //pegando o services alterado do CustomWebApplicationFactory e recuperando a connectString e grando um 
            var configurationServer = webApp.Services.GetService(typeof(IConfiguration));
            ArgumentNullException.ThrowIfNull(configurationServer);
            _configuration = ((IConfiguration)configurationServer);
            _connectionString = _configuration.GetConnectionString("RepositoryMysql");
        }

        protected RepositoryDbContext CreateDBContext(bool preserveData = false)
        {
            if(Configuration.DATABASE_MEMORY)
            {
                var context = new RepositoryDbContext(
                new DbContextOptionsBuilder<RepositoryDbContext>()
                .UseInMemoryDatabase(Configuration.NAME_DATA_BASE)
                .Options
                );

                if (!preserveData)
                    context.Database.EnsureDeleted();

                return context;
            }
            else
            {
                var context = new RepositoryDbContext(
                   new DbContextOptionsBuilder<RepositoryDbContext>()
                   .UseMySql(_connectionString, ServerVersion.AutoDetect(_connectionString))
                   .Options);

                return context;
            }
        }

        protected IDistributedCache CreateCache()
        {
            if (Configuration.DATABASE_MEMORY)
            {
                var services = new ServiceCollection();
                services.AddDistributedMemoryCache();
                var provider = services.BuildServiceProvider();
                var memoryCache = provider.GetService<IDistributedCache>();
                ArgumentNullException.ThrowIfNull(memoryCache) ;
                return memoryCache;
            }
            else
            {
                var redisPassword = _configuration["Redis:Password"];
                var redisEndPoint = _configuration["Redis:Endpoint"];

                var services = new ServiceCollection();
                services.AddStackExchangeRedisCache(options =>
                {
                    options.InstanceName = "Redis";
                    options.Configuration = redisEndPoint;
                    if (!string.IsNullOrEmpty(redisPassword))
                    {
                        options.ConfigurationOptions = new StackExchange.Redis.ConfigurationOptions { Password = redisPassword };
                        options.ConfigurationOptions.EndPoints.Add(redisPassword);
                    }
                });

                var provider = services.BuildServiceProvider();
                var memoryCache = provider.GetService<IDistributedCache>();
                ArgumentNullException.ThrowIfNull(memoryCache) ;
                return memoryCache;
            }
        }

        protected void CleanInMemoryDatabase()
        {
            //CreateDBContext().Database.EnsureDeleted();
        }


        protected void SetupRabbitMQ()
        {
            var channel = webApp.RabbitMQChannel!;
            var options = webApp.RabbitMQConfiguration;

            _setupRabbitMQ = new ServiceRabbitMQ(options, channel);
            _setupRabbitMQ.SetUpWithDeadLetter();
        }

        protected void TearDownRabbitMQ()
        {
            _setupRabbitMQ.Down();
            _setupRabbitMQ.DownDeadLetter();
        }

        public (TEvent?, uint) ReadMessageFromRabbitMQAck<TEvent>() where TEvent : DomainEvent
        {
            var options = webApp.RabbitMQConfiguration;

            //isso apenas para teste por que ele ler apenas um messagem por vez.
            var consumingResult = webApp.RabbitMQChannel!.BasicGet(options.Value.QueueProducts, true);
            
            if (consumingResult is null) return (null,0);

            var rawMessage = consumingResult.Body.ToArray();
            var stringMessage = Encoding.UTF8.GetString(rawMessage);
            var @event = JsonSerializer.Deserialize<TEvent>(stringMessage);

            return (@event, consumingResult.MessageCount);
        }

        public void ReadMessageFromRabbitMQNack<TEvent>() where TEvent : DomainEvent
        {
            var options = webApp.RabbitMQConfiguration;

            //isso apenas para teste por que ele ler apenas um messagem por vez.
            var consumingResult = webApp.RabbitMQChannel!.BasicGet(options.Value.QueueProducts, false);

            webApp.RabbitMQChannel!.BasicNack(consumingResult.DeliveryTag, false, false);
                       
        }

        public (TEvent?, uint) ReadMessageFromRabbitMQDeadLetterQueue<TEvent>() where TEvent : DomainEvent
        {
            var options = webApp.RabbitMQConfiguration;

            var QueueDeadLetter = $"{options.Value.QueueProducts}.DeadLetter";

            //isso apenas para teste por que ele ler apenas um messagem por vez.
            var consumingResult = webApp.RabbitMQChannel!.BasicGet(QueueDeadLetter, true);

            if (consumingResult is null) return (null, 0);

            var rawMessage = consumingResult.Body.ToArray();
            var stringMessage = Encoding.UTF8.GetString(rawMessage);
            var @event = JsonSerializer.Deserialize<TEvent>(stringMessage);

            return (@event, consumingResult.MessageCount);
        }
    }
}
