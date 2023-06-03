using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using MShop.Application.Event;
using MShop.Application.Event.Handler.Products;
using MShop.Business.Events.Products;
using MShop.Business.Interface.Event;
using MShop.Repository.Context;
using MShop.Repository.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MShop.IntegrationTests.Repository.UnitOfOwork
{
    [Collection("Repository UnitOfWork")]
    [CollectionDefinition("Repository UnitOfWork", DisableParallelization = true)]
    public class UnitOfWorkTest : UnitOfWorkTestFixture
    {
        private readonly RepositoryDbContext _DbContext;
        private readonly UnitOfWork _unitOfWork;
        private readonly DomainEventPublisher _domainEventPublisher;
        private readonly Mock<IDomainEventHandler<DomainEventFaker>> _domainEventHandler;
        private readonly ServiceCollection _serviceColletion;

        public UnitOfWorkTest()
        {
            _DbContext = CreateDBContext();
            _domainEventHandler = new Mock<IDomainEventHandler<DomainEventFaker>>();

            _serviceColletion = new ServiceCollection();
            _serviceColletion.AddLogging();
            var serviceProvider = _serviceColletion.BuildServiceProvider();
            
            _domainEventPublisher = new DomainEventPublisher(serviceProvider);
            _unitOfWork = new UnitOfWork(_DbContext, _domainEventPublisher, serviceProvider.GetRequiredService<ILogger<UnitOfWork>>());

        }

        [Fact(DisplayName = nameof(Commit))]
        [Trait("Integration - Infra.Data", "UnitOfWork")]
        public async Task Commit()
        {
            var productExemple = produtcFaker(new Guid());
            await _DbContext.AddAsync(productExemple);
            //productExemple.RegisterEvent( new DomainEventFaker());
            _serviceColletion.AddScoped<IDomainEventHandler<ProductCreatedEvent>, SendToProductCreatedEventHandler>();

            await _unitOfWork.CommitAsync(CancellationToken.None);

            var savedProduct = _DbContext.Products.AsNoTracking().First();
            Assert.Equal(savedProduct.Id, productExemple.Id);
            _domainEventHandler.Verify(x => x.HandlerAsync(It.IsAny<ProductCreatedEvent>()), Times.Once);
            Assert.Equal(0, productExemple.Events.Count);

        }
    }
}
