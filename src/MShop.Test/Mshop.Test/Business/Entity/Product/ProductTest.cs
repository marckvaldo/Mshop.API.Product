using MShop.Business.Exceptions;
using MShop.Business.Validation;
using System.Diagnostics;
using System.Xml.Linq;
using BusinessEntity = MShop.Business.Entity;

namespace Mshop.Test.Business.Entity.Product
{
    public class ProductTest : ProductTestFixture
    {
        [Fact(DisplayName = nameof(Instantiate))]
        [Trait("Business","Products")]
        public void Instantiate()
        {
            var notification = new Notifications();

            var product = GetProductValid();

            product.IsValid(notification);

            Assert.NotNull(product);
            Assert.False(notification.HasErrors());
            Assert.Equal(product.Name, Fake().Name);
            Assert.Equal(product.Description, Fake().Description);
            Assert.Equal(product.Price, Fake().Price);
            Assert.Equal(product.Imagem, Fake().Imagem);
            Assert.Equal(product.CategoryId, Fake().CategoryId);
            Assert.Equal(product.Stock, Fake().Stock);
            Assert.Equal(product.IsActive, Fake().IsActive);  

        }
        
        [Theory(DisplayName = nameof(SholdReturnErrorWhenDescriptionInvalid))]
        [Trait("Business","Products")]
        [InlineData("")]
        [InlineData(null)]
        [InlineData(" ")]
        [InlineData("TV")]

        public void SholdReturnErrorWhenDescriptionInvalid(string description)
        {
            var notification = new Notifications();

            var validade = Fake(
                description,
                "Product Name",
                10,
                "",
                Guid.NewGuid(),
                2,
                true);

            var product = GetProductValid(validade);

            Action action = () => product.IsValid(notification);

            var exception = Assert.Throws<EntityValidationException>(action);

            Assert.True(notification.HasErrors());
            Assert.Equal("Validation errors", exception.Message);

            Assert.Equal(product.Name, validade.Name);
            Assert.Equal(product.Description, description);
            Assert.Equal(product.Price, validade.Price);
            Assert.Equal(product.Imagem, validade.Imagem);
            Assert.Equal(product.CategoryId, validade.CategoryId);
            Assert.Equal(product.Stock, validade.Stock);
            Assert.Equal(product.IsActive, validade.IsActive);
        }


        [Theory(DisplayName = nameof(SholdReturnErrorWhenDescriptionInvalid))]
        [Trait("Business", "Products")]
        [InlineData("")]
        [InlineData(null)]
        [InlineData(" ")]
        [InlineData("TV")]

        public void SholdReturnErrorWhenNameInvalid(string name)
        {
            var notification = new Notifications();

            var validade = Fake(
                "Product Name Product Name",
                name,
                10,
                "",
                Guid.NewGuid(),
                2,
                true
            );

            var product = GetProductValid(validade);

            Action action = () => product.IsValid(notification);

            var exception = Assert.Throws<EntityValidationException>(action);

            Assert.True(notification.HasErrors());
            Assert.Equal("Validation errors", exception.Message);

            Assert.Equal(product.Name, name);
            Assert.Equal(product.Description, validade.Description);
            Assert.Equal(product.Price, validade.Price);
            Assert.Equal(product.Imagem, validade.Imagem);
            Assert.Equal(product.CategoryId, validade.CategoryId);
            Assert.Equal(product.Stock, validade.Stock);
            Assert.Equal(product.IsActive, validade.IsActive);
        }

        [Theory(DisplayName = nameof(SholdReturnErrorWhenDescriptionInvalid))]
        [Trait("Business", "Products")]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(null)]

        public void SholdReturnErrorWhenPriceInvalid(decimal price)
        {
            var notification = new Notifications();

            var validade = Fake(
                "Product Name Product Name",
                "Product Name",
                price,
                "",
                Guid.NewGuid(),
                2,
                true
            );

            var product = GetProductValid(validade);

            Action action = () => product.IsValid(notification);

            var exception = Assert.Throws<EntityValidationException>(action);

            Assert.True(notification.HasErrors());
            Assert.Equal("Validation errors", exception.Message);

            Assert.Equal(product.Name, validade.Name);
            Assert.Equal(product.Description, validade.Description);
            Assert.Equal(product.Price, price);
            Assert.Equal(product.Imagem, validade.Imagem);
            Assert.Equal(product.CategoryId, validade.CategoryId);
            Assert.Equal(product.Stock, validade.Stock);
            Assert.Equal(product.IsActive, validade.IsActive);
        }

        
        [Theory(DisplayName = nameof(SholdActiveAndDeactiveProduct))]
        [Trait("Business","Products")]
        [InlineData(true)]
        [InlineData(false)]
        public void SholdActiveAndDeactiveProduct(bool status)
        {
            var notification = new Notifications();

            var validade = Fake(
               "Product Name Product Name",
               "Product Name",
               10,
               "",
               Guid.NewGuid(),
               2,
               status
           );

            var product = GetProductValid(validade);

            if (status)
                product.Activate();
            else
                product.Deactive();

            product.IsValid(notification);

            
            Assert.Equal(product.IsActive,status);
            Assert.False(notification.HasErrors());

            Assert.Equal(product.Name, validade.Name);
            Assert.Equal(product.Description, validade.Description);
            Assert.Equal(product.Price, validade.Price);
            Assert.Equal(product.Imagem, validade.Imagem);
            Assert.Equal(product.CategoryId, validade.CategoryId);
            Assert.Equal(product.Stock, validade.Stock);
           

        }


        [Fact(DisplayName = nameof(SholdUpdateProduct))]
        [Trait("Business","Products")]
        public void SholdUpdateProduct()
        {
            var notification = new Notifications();

            var newValidade = new
            {
                Name = "Product Name Update",
                Description = "Product Update Product Update",
                Price = 11,
                Imagem = "imagen",
                CategoryId = Guid.NewGuid(),
                stock = 5,
                isActive = true
            };

            var product = GetProductValid();

            product.Update(newValidade.Description, newValidade.Name, newValidade.Price, newValidade.CategoryId);

            product.IsValid(notification);

            Assert.True(product.IsActive);
            Assert.False(notification.HasErrors());

            Assert.Equal(product.Name, newValidade.Name);
            Assert.Equal(product.Description, newValidade.Description);
            Assert.Equal(product.Price, newValidade.Price);
            Assert.Equal(product.CategoryId, newValidade.CategoryId);
        }


        [Fact(DisplayName = nameof(SholdAddQuantityStock))]
        [Trait("Business", "Products")]
        public void SholdAddQuantityStock()
        {
            var notification = new Notifications();



            var newStoque = 10;

            var product = GetProductValid();

            product.AddQuantityStock(newStoque);
            product.IsValid(notification);

            Assert.True(product.IsActive);
            Assert.False(notification.HasErrors());

            Assert.Equal(product.Stock, (newStoque+ Fake().Stock));
        }


        [Fact(DisplayName = nameof(SholdRemoveQuantityStock))]
        [Trait("Business", "Products")]
        public void SholdRemoveQuantityStock()
        {
            var notification = new Notifications();


            var newStoque = 10;

            var product = GetProductValid();

            product.RemoveQuantityStock(newStoque);
            product.IsValid(notification);

            Assert.Equal(product.Stock, (Fake().Stock- newStoque));
            Assert.False(notification.HasErrors());
        }


        [Fact(DisplayName = nameof(SholdUpdateQuantityStock))]
        [Trait("Business", "Products")]
        public void SholdUpdateQuantityStock()
        {
            var notification = new Notifications();


            var newStoque = 1;

            var product = GetProductValid();

            product.UpdateQuantityStock(newStoque);
            product.IsValid(notification);

            Assert.Equal(product.Stock, (newStoque));
            Assert.False(notification.HasErrors());
        }



        [Fact(DisplayName = nameof(SholdUpdateImage))]
        [Trait("Business", "Products")]
        public void SholdUpdateImage()
        {
            var notification = new Notifications();


            var newImagem = "product.jpg";

            var product = GetProductValid();

            product.UpdateImage(newImagem);
            product.IsValid(notification);

            Assert.Equal(product.Imagem, newImagem);
            Assert.False(notification.HasErrors());
        }

    }
}



