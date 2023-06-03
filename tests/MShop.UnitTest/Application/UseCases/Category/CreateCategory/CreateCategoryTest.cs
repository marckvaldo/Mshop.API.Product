using Moq;
using BusinessEntity = MShop.Business.Entity;
using MShop.Business.Interface;
using MShop.Business.Interface.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using useCase = MShop.Application.UseCases.Category.CreateCategory;
using MShop.Business.Exceptions;
using MShop.Business.Validation;
using MShop.Business.Entity;
using MShop.Business.Exception;
using MShop.UnitTests.Application.UseCases.Category.common;

namespace MShop.UnitTests.Application.UseCases.Category.CreateCategory
{
    public class CreateCategoryTest: CreateCategoryTestFituxre
    {
        [Fact(DisplayName = nameof(CreateCategory))]
        [Trait("Application-UseCase","Create Category")]

        public async void CreateCategory()
        {
            var repository = new Mock<ICategoryRepository>();
            var notification = new Mock<INotification>();
            var unitOfWork = new Mock<IUnitOfWork>();

            var request = FakerRequest();

            var useCase = new useCase.CreateCategory(notification.Object, repository.Object,unitOfWork.Object);
            var outPut = await useCase.Handler(request, CancellationToken.None);

            repository.Verify(r => r.Create(It.IsAny<BusinessEntity.Category>(), CancellationToken.None), Times.Once);
            notification.Verify(n => n.AddNotifications(It.IsAny<string>()), Times.Never);

            Assert.NotNull(outPut);
            Assert.Equal(outPut.Name, request.Name);
            Assert.Equal(outPut.IsActive, request.IsActive);
            Assert.NotNull(outPut?.Id);
        }


        [Theory(DisplayName = nameof(CreateCategoryInvalid))]
        [Trait("Application-UseCase", "Create Category")]
        [MemberData(nameof(ListNamesCategoryInvalid))]

        public async void CreateCategoryInvalid(string name)
        {
            var repository = new Mock<ICategoryRepository>();       
            var notification = new Notifications();
            var unitOfWork = new Mock<IUnitOfWork>();

            var request = FakerRequest(name,true);

            var useCase = new useCase.CreateCategory(notification, repository.Object, unitOfWork.Object);
            var action = async () => await useCase.Handler(request, CancellationToken.None);

            var exception = Assert.ThrowsAsync<EntityValidationException>(action);
            repository.Verify(n => n.Create(It.IsAny<BusinessEntity.Category>(), CancellationToken.None), Times.Never);
        }

    }
}
