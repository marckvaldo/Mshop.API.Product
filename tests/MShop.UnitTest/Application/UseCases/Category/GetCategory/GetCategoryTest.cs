using Moq;
using MShop.Business.Interface.Repository;
using MShop.Business.Interface;
using MShop.UnitTests.Application.UseCases.Category.common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApplicationUseCase = MShop.Application.UseCases.Category.GetCategory;
using BusinessEntity = MShop.Business.Entity;
using Bogus;
using MShop.Business.Exception;

namespace MShop.UnitTests.Application.UseCases.Category.GetCategory
{
    public class GetCategoryTest : CategoryBaseFixtureTest
    {
        [Fact(DisplayName = nameof(GetCategory))]
        [Trait("Application-UseCase", "Get Category")]
        public async void GetCategory()
        {
            var repository = new Mock<ICategoryRepository>();
            var notification = new Mock<INotification>();

            var request = Faker();

            repository.Setup(r => r.GetById(It.IsAny<Guid>())).ReturnsAsync(request);

            var useCase = new ApplicationUseCase.GetCategory(notification.Object,repository.Object);
            var outPut =  await useCase.Handle(new ApplicationUseCase.GetCategoryInPut(request.Id), CancellationToken.None);

            Assert.NotNull(outPut);
            Assert.Equal(outPut.Name, request.Name);
            Assert.Equal(outPut.IsActive, request.IsActive);    

        }


        [Fact(DisplayName = nameof(SholdReturnErrorWhenCantGetCategory))]
        [Trait("Application-UseCase", "Get Category")]
        public void SholdReturnErrorWhenCantGetCategory()
        {
            var repository = new Mock<ICategoryRepository>();
            var notification = new Mock<INotification>();


            repository.Setup(r => r.GetById(It.IsAny<Guid>())).ThrowsAsync(new NotFoundException(""));

            var useCase = new ApplicationUseCase.GetCategory(notification.Object, repository.Object);
            var outPut = async () => await useCase.Handle(new ApplicationUseCase.GetCategoryInPut(Guid.NewGuid()), CancellationToken.None);

            var exception = Assert.ThrowsAsync<NotFoundException>(outPut);

            repository.Verify(r => r.GetById(It.IsAny<Guid>()), Times.Once);
            notification.Verify(r => r.AddNotifications(It.IsAny<string>()), Times.Never);
        }
    }
}
