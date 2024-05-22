using Moq;
using MShop.Application.UseCases.Category.ListCategorys;
using MShop.Core.Enum.Paginated;
using MShop.Core.Message;
using MShop.Core.Paginated;
using MShop.Repository.Interface;
using ApplicationUseCases = MShop.Application.UseCases.Category.ListCategorys;
using BusinessEntity = MShop.Business.Entity;

namespace MShop.UnitTests.Application.UseCases.Category.ListCategory
{
    public class ListCategoryTest : ListCategoryFixtureTest
    {
        [Fact(DisplayName = nameof(ListCategory))]
        [Trait("Application-UseCase", "List Categogry")]

        public async void ListCategory()
        {
            var repository = new Mock<ICategoryRepository>();
            var notification = new Mock<INotification>();

            var categorys = FakerCategorys(10);

            var result = new PaginatedOutPut<BusinessEntity.Category>(1, 10, 10, categorys);

            var request = new ListCategoryInPut(1, 10, "", "Name", SearchOrder.Desc);

            repository.Setup(r => r.FilterPaginated(It.IsAny<PaginatedInPut>())).ReturnsAsync(result);

            var useCase = new ApplicationUseCases.ListCategory(repository.Object, notification.Object);
            var outPut = await useCase.Handle(request, CancellationToken.None);

            Assert.NotNull(outPut);
            Assert.NotNull(outPut.Itens);
            Assert.Equal(outPut.PerPage, request.PerPage);
            Assert.Equal(10, outPut.Total);
            Assert.Equal(outPut.Page, request.Page);
            Assert.Equal(10,outPut.Itens.Count);


        }
    }
}
