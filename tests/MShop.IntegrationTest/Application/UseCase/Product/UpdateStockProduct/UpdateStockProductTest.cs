﻿using Moq;
using MShop.Business.Interface.Repository;
using MShop.Business.Interface;
using MShop.IntegrationTests.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MShop.Repository.Repository;
using MShop.Business.Validation;
using MShop.Repository.Context;
using Microsoft.EntityFrameworkCore;
using ApplicationUseCase = MShop.Application.UseCases.Product.UpdateStockProduct;



namespace MShop.IntegrationTests.Application.UseCase.Product.UpdateStockProduct
{
    public class UpdateStockProductTest : UpdateStockProductTestFixture, IDisposable
    {

        private readonly RepositoryDbContext _DbContext;
        private readonly ProductRepository _repository;

        public UpdateStockProductTest()
        {
            _DbContext = CreateDBContext(false, "UpdateStockProductTest");
            _repository = new ProductRepository(_DbContext);
        }

        [Fact(DisplayName = nameof(UpdateStockProduct))]
        [Trait("Integration-Infra.Data", "Product Use Case")]

        public async Task UpdateStockProduct()
        {
            var notification = new Notifications();

            var product = Faker();
            var request = RequestFake();

            await _DbContext.AddAsync(product);
            await _DbContext.SaveChangesAsync();

            var useCase = new ApplicationUseCase.UpdateStockProducts(_repository, notification);
            var outPut = await useCase.Handle(request);

            var productDb = await _DbContext.Products.AsNoTracking().Where(x => x.Id == product.Id).FirstAsync();

            Assert.NotNull(outPut);
            Assert.Equal(request.Stock, outPut.Stock);

        }

        public void Dispose()
        {
            CleanInMemoryDatabase(_DbContext);
        }
    }
}
