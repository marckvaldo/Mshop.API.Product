using Microsoft.EntityFrameworkCore;
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

        public UnitOfWorkTest()
        {
            _DbContext = CreateDBContext();
        }

        [Fact(DisplayName = nameof(Commit))]
        [Trait("Integration - Infra.Data", "UnitOfWork")]
        public async Task Commit()
        {
            var productExemple = produtcFaker(new Guid());
            await _DbContext.AddAsync(productExemple);

            var unitOfWork = new UnitOfWork(_DbContext);

            await unitOfWork.CommitAsync(CancellationToken.None);

            var savedProduct = _DbContext.Products.AsNoTracking().First();
            Assert.Equal(savedProduct.Id, productExemple.Id);   

        }
    }
}
