using BusinessEntity = MShop.Business.Entity;
using BusinessExceptions = MShop.Business.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MShop.Business.Validation;
using System.Diagnostics;
using MShop.Business.Entity;

namespace Mshop.Test.Business.Entity.Category
{
    public class CategoryTest
    {
        [Fact(DisplayName = nameof(Instantiate))]
        [Trait("Business","Category")]
        public void Instantiate()
        {
            var notification = new Notifications();

            var validate = new
            {
                Name = "Category Name",
                isActive = true,
                isValid = true
            };

            var category = new BusinessEntity.Category(validate.Name, validate.isActive);
            category.IsValid(notification);

            Assert.False(notification.HasErrors());
            Assert.NotNull(category);
            Assert.Equal(validate.Name, category.Name);
            Assert.Equal(validate.isActive, category.IsActive);
            Assert.NotEqual(Guid.Empty, category.Id);

        }

        [Theory(DisplayName = nameof(SholdReturnErroWhenNameIsInvalid))]
        [Trait("Business","Category")]
        [InlineData("")]
        [InlineData(null)]
        [InlineData(" ")]
        [InlineData("T")]
        [InlineData("TV")]
        public void SholdReturnErroWhenNameIsInvalid(string? name)
        {
            var notification = new Notifications();
            
            var validate = new
            {
                Name = "Category Name",
                isActive = true,
                isValid = false
            };

            var category = new BusinessEntity.Category(name);
            Action action =
                () => category.IsValid(notification);

            var exception = Assert.Throws<BusinessExceptions.EntityValidationException>(action);

            Assert.Equal("Validation errors", exception.Message);
            Assert.True(notification.HasErrors());

        }

        [Fact(DisplayName = nameof(SholdActivateCategory))]
        [Trait("Business","Category")]
        public void SholdActivateCategory()
        {
            var notification = new Notifications();

            var validate = new
            {
                Name = "Category Name",
                isActive = true,
                isValid = true
            };

            var category = new BusinessEntity.Category(validate.Name, false);
            category.Active();
            category.IsValid(notification);

            
            Assert.Equal(validate.isActive, category.IsActive);
            Assert.False(notification.HasErrors());

        }

        [Fact(DisplayName = nameof(SholdDeactiveCategory))]
        [Trait("Business","Category")]
        public void SholdDeactiveCategory()
        {
            var notification = new Notifications();

            var validate = new
            {
                Name = "Category Name",
                isActive = false,
                isValid = true
            };

            var category = new BusinessEntity.Category(validate.Name, true);
            category.Deactive();
            category.IsValid(notification);

            Assert.Equal(validate.isActive, category.IsActive);
            Assert.False(notification.HasErrors());

        }


        [Fact(DisplayName = nameof(SholdUpdateCategory))]
        [Trait("Business","Category")]
        public void SholdUpdateCategory()
        {
            var notification = new Notifications();

            var validade = new
            {
                Name = "Category Name",
                isActive = true,
                isValid = true
            };

            var newValidade = new
            {
                Name = "Category New"
            };

            var category = new BusinessEntity.Category(validade.Name);
            category.Update(newValidade.Name);
            category.IsValid(notification);

            Assert.False(notification.HasErrors());
            Assert.Equal(category.Name, newValidade.Name);
        }
    }
}
