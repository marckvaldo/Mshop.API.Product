using BusinessExceptions = MShop.Business.Exceptions;
using MShop.Business.Validation;
using MShop.UnitTests.Common;

namespace Mshop.Test.Business.Entity.Category
{
    public class CategoryTest : CategoryTestFixture
    { 
        [Fact(DisplayName = nameof(Instantiate))]
        [Trait("Business","Category")]
        public void Instantiate()
        {
            var notification = new Notifications();
            var valid = GetCategoryValid();

            var category = GetCategoryValid(valid.Name, valid.IsActive); ;
            category.IsValid(notification);

            Assert.False(notification.HasErrors());
            Assert.NotNull(category);
            Assert.Equal(valid.Name, category.Name);
            Assert.Equal(valid.IsActive, category.IsActive);
            Assert.NotEqual(Guid.Empty, category.Id);

        }

        [Theory(DisplayName = nameof(SholdReturnErroWhenNameIsInvalid))]
        [Trait("Business","Category")]
        [MemberData(nameof(ListNamesCategoryInvalid))]
        public void SholdReturnErroWhenNameIsInvalid(string? name)
        {
            var notification = new Notifications();
            var category = GetCategoryValid(name);
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
            var category = GetCategoryValid(Fake().Name, false);
            category.Active();
            category.IsValid(notification);

            Assert.True(category.IsActive);
            Assert.False(notification.HasErrors());
        }

        [Fact(DisplayName = nameof(SholdDeactiveCategory))]
        [Trait("Business","Category")]
        public void SholdDeactiveCategory()
        {
            var notification = new Notifications();
            var category = GetCategoryValid(Fake().Name, true);
            category.Deactive();
            category.IsValid(notification);

            Assert.False(category.IsActive);
            Assert.False(notification.HasErrors());
        }


        [Fact(DisplayName = nameof(SholdUpdateCategory))]
        [Trait("Business","Category")]
        public void SholdUpdateCategory()
        {
            var notification = new Notifications();
            var newValidade = new
            {
                Name = "Category New"
            };

            var category = GetCategoryValid(); //new BusinessEntity.Category(validade.Name);
            category.Update(newValidade.Name);
            category.IsValid(notification);

            Assert.False(notification.HasErrors());
            Assert.Equal(category.Name, newValidade.Name);
        }
    }
}
