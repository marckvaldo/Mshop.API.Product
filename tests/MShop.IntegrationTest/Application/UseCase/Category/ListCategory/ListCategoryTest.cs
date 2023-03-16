using MShop.Business.Interface.Repository;
using MShop.Business.Interface;
using MShop.Repository.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MShop.Business.Validation;
using MShop.Repository.Repository;
using MShop.IntegrationTests.Application.UseCase.Product.Common;
using Microsoft.EntityFrameworkCore;
using MShop.Application.UseCases.Product.ListProducts;
using MShop.Business.Enum.Paginated;
using MShop.IntegrationTests.Application.UseCase.Category.Common;
using ApplicationUseCase = MShop.Application.UseCases.Category.ListCategorys;
using MShop.Application.UseCases.Category.ListCategorys;

namespace MShop.IntegrationTests.Application.UseCase.Category.ListCategory
{
    [Collection("List Category Collection")]
    [CollectionDefinition("List Category Collection", DisableParallelization = true)]
    public class ListCategoryTest : CategoryTestFixture, IDisposable
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IProductRepository _productRepository;
        private readonly INotification _notification;
        private readonly RepositoryDbContext _context;
        private readonly CategoryPersistence _categoryPersistence;

        public ListCategoryTest()
        {
            _context = CreateDBContext();
            _categoryRepository = new CategoryRepository(_context);
            _productRepository = new ProductRepository(_context);
            _notification = new Notifications();
            _categoryPersistence = new CategoryPersistence(_context);
        }


        [Fact(DisplayName = nameof(ListCatgories))]
        [Trait("Integration-Application", "Category Use Case")]

        public async Task ListCatgories()
        {

            var categoryFake = FakerList(20);
            await _categoryPersistence.CreateList(categoryFake);

            var useCase = new ApplicationUseCase.ListCategory(_categoryRepository, _notification);
            var request = new ListCategoryInPut(
                            page: 1,
                            perPage: 5,
                            search: "",
                            sort: "name",
                            dir: SearchOrder.Asc
                            );

            var outPut = await useCase.Handler(request);

            Assert.NotNull(outPut);
            Assert.Equal(categoryFake.Count, outPut.Total);
            Assert.Equal(request.Page, outPut.Page);
            Assert.Equal(request.PerPage, outPut.PerPage);
            Assert.NotNull(outPut.Itens);
            Assert.True(outPut.Itens.Any());

        }

        public void Dispose()
        {
            CleanInMemoryDatabase();
        }

    }
}
