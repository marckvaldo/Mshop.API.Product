using Moq;
using MShop.Core.Data;
using MShop.Core.Exception;
using MShop.Core.Message;
using MShop.Repository.Interface;
using BusinessEntity = MShop.Business.Entity;
using useCase = MShop.Application.UseCases.Category.CreateCategory;

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
            var outPut = await useCase.Handle(request, CancellationToken.None);

            repository.Verify(r => r.Create(It.IsAny<BusinessEntity.Category>(), CancellationToken.None), Times.Once);
            notification.Verify(n => n.AddNotifications(It.IsAny<string>()), Times.Never);
            unitOfWork.Verify(r => r.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);

            Assert.NotNull(outPut.Data);
            Assert.Equal(outPut?.Data?.Name, request.Name);
            Assert.Equal(outPut?.Data?.IsActive, request.IsActive);
            Assert.NotNull(outPut?.Data?.Id);
        }


        [Theory(DisplayName = nameof(CreateCategoryInvalid))]
        [Trait("Application-UseCase", "Create Category")]
        [MemberData(nameof(ListNamesCategoryInvalid))]

        public void CreateCategoryInvalid(string name)
        {
            var repository = new Mock<ICategoryRepository>();       
            var notification = new Notifications();
            var unitOfWork = new Mock<IUnitOfWork>();

            var request = FakerRequest(name,true);

            var useCase = new useCase.CreateCategory(notification, repository.Object, unitOfWork.Object);
            var action = async () => await useCase.Handle(request, CancellationToken.None);

            var exception = Assert.ThrowsAsync<EntityValidationException>(action);
            repository.Verify(n => n.Create(It.IsAny<BusinessEntity.Category>(), CancellationToken.None), Times.Never);
            unitOfWork.Verify(r => r.CommitAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

    }
}
