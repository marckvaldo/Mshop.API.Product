﻿using MShop.Business.Events.Products;
using MShop.Core.DomainObject;
using MShop.Core.Exception;
using MShop.Core.Message;

namespace Mshop.Test.Business.Entity.Product
{
    public class ProductTest : ProductTestFixture, IDisposable
    {
        private readonly Notifications _notifications;
        public ProductTest()
        {
            _notifications = new Notifications();
        }

        [Fact(DisplayName = nameof(Instantiate))]
        [Trait("Business","Products")]
        public void Instantiate()
        {
            var valid = GetProductValid();
            var product = GetProductValid(Fake(valid.Description,valid.Name,valid.Price, valid.CategoryId, valid.Stock,valid.IsActive));
            product.IsValid(_notifications);

            product.ProductCreatedEvent(new ProductCreatedEvent(
                product.Id,
                product.Description,
                product.Name,
                product.Price,
                product.Stock,
                product.IsActive,
                product.CategoryId,
                product?.Category?.Name,
                product.Thumb?.Path,
                product.IsSale)); ;

            Assert.NotNull(product);
            Assert.False(_notifications.HasErrors());
            Assert.Equal(product.Name, valid.Name);
            Assert.Equal(product.Description, valid.Description);
            Assert.Equal(product.Price, valid.Price);
            Assert.Equal(product.Thumb, valid.Thumb);
            Assert.Equal(product.CategoryId, valid.CategoryId);
            Assert.Equal(product.Stock, valid.Stock);
            Assert.Equal(product.IsActive, valid.IsActive);
            Assert.Null(product.Thumb);

            Assert.Equal(1, product.Events.Count);

            foreach (DomainEvent @event in product.Events)
            {
                Assert.Equal((dynamic)@event.GetType().Name, nameof(ProductCreatedEvent));
            }

        }
        
        [Theory(DisplayName = nameof(SholdReturnErrorWhenDescriptionInvalid))]
        [Trait("Business","Products")]
        [MemberData(nameof(ListDescriptionProductInvalid))]

        public void SholdReturnErrorWhenDescriptionInvalid(string description)
        {

            var validade = Fake(
                description,
                Fake().Name,
                Fake().Price,
                Fake().CategoryId,
                Fake().Stock,
                Fake().IsActive);

            var product = GetProductValid(validade);
            product.Activate();

            //Action action = () => product.IsValid(_notifications);
            //var exception = Assert.Throws<EntityValidationException>(action);
            //Assert.Equal("Validation errors", exception.Message);

            Assert.False(product.IsValid(_notifications));
            Assert.True(_notifications.HasErrors());
            
            

            Assert.Equal(product.Name, validade.Name);
            Assert.Equal(product.Description, description);
            Assert.Equal(product.Price, validade.Price);
            Assert.Equal(product.CategoryId, validade.CategoryId);
            Assert.Equal(product.Stock, validade.Stock);
            Assert.Equal(product.IsActive, validade.IsActive);
            Assert.Null(product.Thumb);
            Assert.Equal(0, product.Events.Count);
        }


        [Theory(DisplayName = nameof(SholdReturnErrorWhenNameInvalid))]
        [Trait("Business", "Products")]
        [MemberData(nameof(ListNameProductInvalid))]
        public void SholdReturnErrorWhenNameInvalid(string name)
        {
           
            var validade = Fake(
                Fake().Description,
                name,
                Fake().Price,
                Fake().CategoryId,
                Fake().Stock,
                Fake().IsActive);

            var product = GetProductValid(validade);
            product.Activate();

            //Action action = () => product.IsValid(_notifications);
            //var exception = Assert.Throws<EntityValidationException>(action);
            //Assert.Equal("Validation errors", exception.Message);

            Assert.False(product.IsValid(_notifications));
            Assert.True(_notifications.HasErrors());
            
            Assert.Equal(product.Name, name);
            Assert.Equal(product.Description, validade.Description);
            Assert.Equal(product.Price, validade.Price);
            Assert.Equal(product.CategoryId, validade.CategoryId);
            Assert.Equal(product.Stock, validade.Stock);
            Assert.Equal(product.IsActive, validade.IsActive);
            Assert.Null(product.Thumb);
            Assert.Equal(0, product.Events.Count);
        }

        [Theory(DisplayName = nameof(SholdReturnErrorWhenPriceInvalid))]
        [Trait("Business", "Products")]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(null)]

        public void SholdReturnErrorWhenPriceInvalid(decimal price)
        {

            var validade = Fake(
                Fake().Description,
                Fake().Name,
                price,
                Fake().CategoryId,
                Fake().Stock,
                Fake().IsActive);

            var product = GetProductValid(validade);
            product.Activate();

            //Action action = () => product.IsValid(_notifications);
            //var exception = Assert.Throws<EntityValidationException>(action);
            //Assert.Equal("Validation errors", exception.Message);

            Assert.False(product.IsValid(_notifications));
            Assert.True(_notifications.HasErrors());
            
            Assert.Equal(product.Name, validade.Name);
            Assert.Equal(product.Description, validade.Description);
            Assert.Equal(product.Price, price);
            Assert.Equal(product.CategoryId, validade.CategoryId);
            Assert.Equal(product.Stock, validade.Stock);
            Assert.Equal(product.IsActive, validade.IsActive);
            Assert.Null(product.Thumb);
            Assert.Equal(0, product.Events.Count);

        }

        
        [Theory(DisplayName = nameof(SholdActiveAndDeactiveProduct))]
        [Trait("Business","Products")]
        [InlineData(true)]
        [InlineData(false)]
        public void SholdActiveAndDeactiveProduct(bool status)
        {
           
            var validade = Fake(
               Fake().Description,
               Fake().Name,
               Fake().Price,
               Fake().CategoryId,
               Fake().Stock,
               status);


            var product = GetProductValid(validade);

            if (status)
                product.Activate();
            else
                product.Deactive();

            product.IsValid(_notifications);

            
            Assert.Equal(product.IsActive,status);
            Assert.False(_notifications.HasErrors());
            Assert.Equal(product.Name, validade.Name);
            Assert.Equal(product.Description, validade.Description);
            Assert.Equal(product.Price, validade.Price);
            Assert.Equal(product.CategoryId, validade.CategoryId);
            Assert.Equal(product.Stock, validade.Stock);
            Assert.Null(product.Thumb);
            Assert.Equal(0, product.Events.Count);


        }


        [Fact(DisplayName = nameof(SholdUpdateProduct))]
        [Trait("Business","Products")]
        public void SholdUpdateProduct()
        {

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

            product.IsValid(_notifications);
            product.ProductUpdatedEvent(new ProductUpdatedEvent(product.Id,
                product.Description,
                product.Name,
                product.Price,
                product.Stock,
                product.IsActive,
                product.CategoryId,
                "Categoria",
                product.Thumb?.Path,
                product.IsSale)) ;

            Assert.True(product.IsActive);
            Assert.False(_notifications.HasErrors());
            Assert.Equal(product.Name, newValidade.Name);
            Assert.Equal(product.Description, newValidade.Description);
            Assert.Equal(product.Price, newValidade.Price);
            Assert.Equal(product.CategoryId, newValidade.CategoryId);
            Assert.Null(product.Thumb);
            Assert.NotEmpty(product.Events);
            Assert.Equal(1, product.Events.Count);

            foreach (DomainEvent @event in product.Events)
            {
                Assert.Equal((dynamic)@event.GetType().Name, nameof(ProductUpdatedEvent));
            }

        }


        [Fact(DisplayName = nameof(SholdAddQuantityStock))]
        [Trait("Business", "Products")]
        public void SholdAddQuantityStock()
        {
            var validate = Fake();
            var newStoque = 10;
            var product = GetProductValid(validate);

            product.AddQuantityStock(newStoque);
            product.IsValid(_notifications);

            product.ProductUpdatedEvent(new ProductUpdatedEvent(product.Id,
                product.Description,
                product.Name,
                product.Price,
                product.Stock,
                product.IsActive,
                product.CategoryId,
                "Cantegoria",
                product.Thumb?.Path,
                product.IsSale));

            Assert.True(product.IsActive);
            Assert.False(_notifications.HasErrors());
            Assert.Equal(product.Stock, (newStoque+ validate.Stock));
            Assert.NotEmpty(product.Events);
            Assert.Equal(1, product.Events.Count);

            foreach (DomainEvent @event in product.Events)
            {
                Assert.Equal((dynamic)@event.GetType().Name, nameof(ProductUpdatedEvent));
            }


        }


        [Fact(DisplayName = nameof(SholdRemoveQuantityStock))]
        [Trait("Business", "Products")]
        public void SholdRemoveQuantityStock()
        {
            var validate = Fake();
            var newStoque = 10;
            var product = GetProductValid(validate);

            product.RemoveQuantityStock(newStoque);
            product.IsValid(_notifications);

            product.ProductUpdatedEvent(new ProductUpdatedEvent(product.Id,
                product.Description,
                product.Name,
                product.Price,
                product.Stock,
                product.IsActive,
                product.CategoryId,
                "Categoria",
                product.Thumb?.Path,
                product.IsSale));

            Assert.Equal(product.Stock, (validate.Stock- newStoque));
            Assert.False(_notifications.HasErrors());
            Assert.NotEmpty(product.Events);
            Assert.Equal(1, product.Events.Count);

            foreach (DomainEvent @event in product.Events)
            {
                Assert.Equal((dynamic)@event.GetType().Name, nameof(ProductUpdatedEvent));
            }

        }


        [Fact(DisplayName = nameof(SholdUpdateQuantityStock))]
        [Trait("Business", "Products")]
        public void SholdUpdateQuantityStock()
        {
            var newStoque = 1;
            var product = GetProductValid();

            product.UpdateQuantityStock(newStoque);
            product.IsValid(_notifications);

            product.ProductUpdatedEvent(new ProductUpdatedEvent(product.Id,
                product.Description,
                product.Name,
                product.Price,
                product.Stock,
                product.IsActive,
                product.CategoryId,
                "Categoria",
                product.Thumb?.Path,
                product.IsSale));

            Assert.Equal(product.Stock, (newStoque));
            Assert.False(_notifications.HasErrors());
            Assert.NotEmpty(product.Events);
            Assert.Equal(1, product.Events.Count);

            foreach(DomainEvent @event in product.Events)
            {
                Assert.Equal((dynamic)@event.GetType().Name, nameof(ProductUpdatedEvent));
            }
           

        }



        [Fact(DisplayName = nameof(SholdUpdateImage))]
        [Trait("Business", "Products")]
        public void SholdUpdateImage()
        {            
            var newImagem = faker.Image.LoremFlickrUrl();
            var product = GetProductValid();

            product.UpdateThumb(newImagem);
            product.IsValid(_notifications);

            product.ProductUpdatedEvent(new ProductUpdatedEvent(product.Id,
                product.Description,
                product.Name,
                product.Price,
                product.Stock,
                product.IsActive,
                product.CategoryId,
                "Categoria",
                product.Thumb?.Path,
                product.IsSale));

            Assert.Equal(product.Thumb.Path, newImagem);
            Assert.False(_notifications.HasErrors());            
            Assert.NotEmpty(product.Events);
            Assert.Equal(1, product.Events.Count);

            foreach (DomainEvent @event in product.Events)
            {
                Assert.Equal((dynamic)@event.GetType().Name, nameof(ProductUpdatedEvent));
            }



        }

        public void Dispose()
        {
            _notifications.Errors().Clear();
        }
    }
}



