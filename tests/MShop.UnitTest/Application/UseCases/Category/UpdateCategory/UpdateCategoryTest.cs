using Moq;
using MShop.Core.Data;
using MShop.Core.Exception;
using MShop.Core.Message;
using MShop.Repository.Interface;
using ApplicationUseCase = MShop.Application.UseCases.Category.UpdateCategory;
using BusinessEntity = MShop.Business.Entity;

namespace MShop.UnitTests.Application.UseCases.Category.UpdateCategory
{
    public class UpdateCategoryTest : UpdateCategoryTestFituxre
    {
        private readonly Mock<INotification> _notifications;
        private readonly Mock<IUnitOfWork> _unitOfWork;
        private readonly Mock<ICategoryRepository> _repositoryCategory;

        public UpdateCategoryTest()
        {
            _notifications = new Mock<INotification>();
            _unitOfWork = new Mock<IUnitOfWork>();           
            _repositoryCategory = new Mock<ICategoryRepository>();
        }

        [Fact(DisplayName = nameof(UpdateCategory))]
        [Trait("Application-UseCase", "Update Category")]
        public async void UpdateCategory()
        {  
            var request = FakerRequest();
            var productFake = Faker();

            _repositoryCategory.Setup(r => r.GetById(It.IsAny<Guid>()))
                .ReturnsAsync(productFake);

            var useCase = new ApplicationUseCase.UpdateCategory(
                _repositoryCategory.Object,
                _notifications.Object,
                _unitOfWork.Object);

            var outPut = await useCase.Handle(request, CancellationToken.None);


            _repositoryCategory.Verify(r => r.Update(It.IsAny<BusinessEntity.Category>(), CancellationToken.None), Times.Once);
            _notifications.Verify(n => n.AddNotifications(It.IsAny<string>()), Times.Never);
            _unitOfWork.Verify(u => u.CommitAsync(It.IsAny<CancellationToken>()),Times.Once);

            var result = outPut.Data;

            Assert.NotNull(result);
            Assert.Equal(result.Name, request.Name);
            Assert.Equal(result.IsActive, request.IsActive);

        }

        [Fact(DisplayName = nameof(SholdReturnErrorWhenUpdateCategory))]
        [Trait("Application-UseCase", "Update Category")]
        public void SholdReturnErrorWhenUpdateCategory()
        {
            var request = FakerRequest();
            var productFake = Faker();

            _repositoryCategory.Setup(r=>r.GetById(It.IsAny<Guid>())).ThrowsAsync(new NotFoundException("")); 

            var useCase = new ApplicationUseCase.UpdateCategory(
                _repositoryCategory.Object,
                _notifications.Object,
                _unitOfWork.Object);

            var action = async () => await useCase.Handle(request, CancellationToken.None);

            var exception = Assert.ThrowsAsync<NotFoundException>(action);

            _repositoryCategory.Verify(r => r.Update(It.IsAny<BusinessEntity.Category>(), CancellationToken.None), Times.Never);
            _notifications.Verify(n => n.AddNotifications(It.IsAny<string>()), Times.Never);
            _unitOfWork.Verify(u=>u.CommitAsync(It.IsAny<CancellationToken>()), Times.Never);

        }
    }
}
