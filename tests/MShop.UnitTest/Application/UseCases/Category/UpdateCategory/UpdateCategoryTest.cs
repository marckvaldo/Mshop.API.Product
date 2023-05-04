using Moq;
using MShop.Business.Interface.Repository;
using MShop.Business.Interface.Service;
using MShop.Business.Interface;
using MShop.UnitTests.Application.UseCases.Category.common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApplicationUseCase = MShop.Application.UseCases.Category.UpdateCategory;
using BusinessEntity = MShop.Business.Entity;
using MShop.Business.Exceptions;
using MShop.Business.Exception;

namespace MShop.UnitTests.Application.UseCases.Category.UpdateCategory
{
    public class UpdateCategoryTest : UpdateCategoryTestFituxre
    {
        [Fact(DisplayName = nameof(UpdateCategory))]
        [Trait("Application-UseCase", "Update Category")]
        public async void UpdateCategory()
        {
            var repository = new Mock<ICategoryRepository>();
            var notification = new Mock<INotification>();
  
            var request = FakerRequest();

            var productFake = Faker();

            repository.Setup(r => r.GetById(It.IsAny<Guid>()))
                .ReturnsAsync(productFake);

            var useCase = new ApplicationUseCase.UpdateCategory(repository.Object, notification.Object);
            var outPut = await useCase.Handler(request);


            repository.Verify(r => r.Update(It.IsAny<BusinessEntity.Category>()), Times.Once);
            notification.Verify(n => n.AddNotifications(It.IsAny<string>()), Times.Never);

            Assert.NotNull(outPut);
            Assert.Equal(outPut.Name, request.Name);
            Assert.Equal(outPut.IsActive, request.IsActive);

        }

        [Fact(DisplayName = nameof(SholdReturnErrorWhenUpdateCategory))]
        [Trait("Application-UseCase", "Update Category")]
        public void SholdReturnErrorWhenUpdateCategory()
        {
            var repository = new Mock<ICategoryRepository>();
            var notification = new Mock<INotification>();

            var request = FakerRequest();

            var productFake = Faker();

            repository.Setup(r=>r.GetById(It.IsAny<Guid>())).ThrowsAsync(new NotFoundException("")); 

            var useCase = new ApplicationUseCase.UpdateCategory(repository.Object, notification.Object);
            var action = async () => await useCase.Handler(request);

            var exception = Assert.ThrowsAsync<NotFoundException>(action);

            repository.Verify(r => r.Update(It.IsAny<BusinessEntity.Category>()), Times.Never);
            notification.Verify(n => n.AddNotifications(It.IsAny<string>()), Times.Never);


        }
    }
}
