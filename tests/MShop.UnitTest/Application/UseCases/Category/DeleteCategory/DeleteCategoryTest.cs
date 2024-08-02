﻿using Moq;
using MShop.Core.Data;
using MShop.Core.Exception;
using MShop.Core.Message;
using MShop.Repository.Interface;
using BusinessEntity = MShop.Business.Entity;
using useCase = MShop.Application.UseCases.Category.DeleteCategory;

namespace MShop.UnitTests.Application.UseCases.Category.DeleteCategory
{
    public class DeleteCategoryTest : DeleteCategoryTestFixture
    {
        [Fact(DisplayName = nameof(DeleteCategory))]
        [Trait("Application-UseCase", "Delete Category")]

        public async void DeleteCategory()
        {
            var repository = new Mock<ICategoryRepository>();
            var notification = new Mock<INotification>();
            var repositoryProduct = new Mock<IProductRepository>();
            var unitOfWork = new Mock<IUnitOfWork>();

            var category = Faker();

            //repositoryProduct.Setup(r => r.GetProductsByCategoryId(It.IsAny<Guid>())).ReturnsAsync(FakerProducts(6,FakerCategory()));
            repository.Setup(r => r.GetById(It.IsAny<Guid>())).ReturnsAsync(category);

            var useCase = new useCase.DeleteCategory(
                repository.Object, 
                repositoryProduct.Object, 
                notification.Object,
                unitOfWork.Object);

            var outPut = await useCase.Handle(new useCase.DeleteCategoryInPut(category.Id), CancellationToken.None);

            repository.Verify(r => r.GetById(It.IsAny<Guid>()), Times.Once);
            notification.Verify(n => n.AddNotifications(It.IsAny<string>()), Times.Never);
            unitOfWork.Verify(r => r.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);

            var result = outPut.Data;

            Assert.NotNull(result);
            Assert.Equal(result.Name, category.Name);
            Assert.Equal(result.IsActive, category.IsActive);
            Assert.NotNull(result?.Id);
        }



        [Fact(DisplayName = nameof(ShouldReturnErroWhenDeleteCategoryNotFound))]
        [Trait("Application-UseCase", "Delete Category")]

        public void ShouldReturnErroWhenDeleteCategoryNotFound()
        {
            var repository = new Mock<ICategoryRepository>();
            var notification = new Mock<INotification>();
            var repositoryProduct = new Mock<IProductRepository>();
            var unitOfWork = new Mock<IUnitOfWork>();

            var category = Faker();

            var useCase = new useCase.DeleteCategory(
                repository.Object, 
                repositoryProduct.Object, 
                notification.Object,
                unitOfWork.Object);

            var action = async () => await useCase.Handle(new useCase.DeleteCategoryInPut(category.Id), CancellationToken.None);

            var exception = Assert.ThrowsAsync<ApplicationValidationException>(action);
            repository.Verify(n => n.DeleteById(It.IsAny<BusinessEntity.Category>(), CancellationToken.None), Times.Never);          
            notification.Verify(n => n.AddNotifications(It.IsAny<string>()), Times.Once);
            repositoryProduct.Verify(n => n.GetProductsByCategoryId(It.IsAny<Guid>()), Times.Never);
            unitOfWork.Verify(r => r.CommitAsync(It.IsAny<CancellationToken>()), Times.Never);

        }



        [Fact(DisplayName = nameof(ShouldReturnErroWhenDeleteCategoryWhenThereAreSameProducts))]
        [Trait("Application-UseCase", "Delete Category")]

        public void ShouldReturnErroWhenDeleteCategoryWhenThereAreSameProducts()
        {
            var repository = new Mock<ICategoryRepository>();
            var repositoryProduct = new Mock<IProductRepository>();
            var notification = new Mock<INotification>();
            var unitOfWork = new Mock<IUnitOfWork>();

            var category = Faker();
            var product = FakerProducts(3, category);

            repository.Setup(r => r.GetById(It.IsAny<Guid>())).ReturnsAsync(category);
            repositoryProduct.Setup(r => r.GetProductsByCategoryId(It.IsAny<Guid>())).ReturnsAsync(product);
            

            var useCase = new useCase.DeleteCategory(
                repository.Object, 
                repositoryProduct.Object, 
                notification.Object, 
                unitOfWork.Object);

            var action = async () => await useCase.Handle(new useCase.DeleteCategoryInPut(category.Id), CancellationToken.None);

            var exception = Assert.ThrowsAsync<ApplicationValidationException>(action);
            repository.Verify(n => n.GetById(It.IsAny<Guid>()), Times.Once);
            repository.Verify(n => n.DeleteById(It.IsAny<BusinessEntity.Category>(), CancellationToken.None), Times.Never);
            notification.Verify(n => n.AddNotifications(It.IsAny<string>()), Times.Once);
            unitOfWork.Verify(r => r.CommitAsync(It.IsAny<CancellationToken>()), Times.Never);

        }
    }
}
