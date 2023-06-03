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

            var outPut = await useCase.Handler(request, CancellationToken.None);


            _repositoryCategory.Verify(r => r.Update(It.IsAny<BusinessEntity.Category>(), CancellationToken.None), Times.Once);
            _notifications.Verify(n => n.AddNotifications(It.IsAny<string>()), Times.Never);
            _unitOfWork.Verify(u => u.CommitAsync(It.IsAny<CancellationToken>()),Times.Once);

            Assert.NotNull(outPut);
            Assert.Equal(outPut.Name, request.Name);
            Assert.Equal(outPut.IsActive, request.IsActive);

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

            var action = async () => await useCase.Handler(request, CancellationToken.None);

            var exception = Assert.ThrowsAsync<NotFoundException>(action);

            _repositoryCategory.Verify(r => r.Update(It.IsAny<BusinessEntity.Category>(), CancellationToken.None), Times.Never);
            _notifications.Verify(n => n.AddNotifications(It.IsAny<string>()), Times.Never);
            _unitOfWork.Verify(u=>u.CommitAsync(It.IsAny<CancellationToken>()), Times.Never);

        }
    }
}
