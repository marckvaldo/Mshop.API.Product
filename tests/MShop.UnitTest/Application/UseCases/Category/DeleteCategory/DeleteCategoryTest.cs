﻿using Moq;
using MShop.Business.Interface.Repository;
using MShop.Business.Interface;
using MShop.UnitTests.Application.UseCases.Category.common;
using BusinessEntity = MShop.Business.Entity;
using useCase = MShop.Application.UseCases.Category.DeleteCategory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MShop.Business.Exceptions;

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

            Assert.NotNull(outPut);
            Assert.Equal(outPut.Name, category.Name);
            Assert.Equal(outPut.IsActive, category.IsActive);
            Assert.NotNull(outPut?.Id);
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
