﻿using Moq;
using MShop.Core.Exception;
using MShop.Core.Message;
using MShop.Repository.Interface;
using MShop.UnitTests.Application.UseCases.Category.common;
using ApplicationUseCase = MShop.Application.UseCases.Category.GetCategory;

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

            var result = outPut.Data;

            Assert.NotNull(result);
            Assert.Equal(result.Name, request.Name);
            Assert.Equal(result.IsActive, request.IsActive);    

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
