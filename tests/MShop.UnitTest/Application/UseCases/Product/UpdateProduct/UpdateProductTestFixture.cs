using Bogus;
using Mshop.Test.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessEntity = MShop.Business.Entity;
using ApplicationUseCase = MShop.Application.UseCases.Product.UpdateProduct;
using MShop.Application.UseCases.Product.Common;
using MShop.Business.ValueObject;
using MShop.Application.Common;
using MShop.UnitTests.Common;

namespace Mshop.Tests.Application.UseCases.Product.UpdateProduct
{
    public class UpdateProductTestFixture : BaseFixture
    {
        private readonly Guid _categoryId;
        private readonly Guid _id;
        public UpdateProductTestFixture() : base()
        {
            _categoryId = Guid.NewGuid();
            _id = Guid.NewGuid();
        }

        protected static FileInput ImageFake()
        {
            return new FileInput("jpg", new MemoryStream(Encoding.ASCII.GetBytes(fakerStatic.Image.LoremPixelUrl())));
        }
       

        protected ApplicationUseCase.UpdateProductInPut ProductInPut()
        {
            return new ApplicationUseCase.UpdateProductInPut
            {
                Id = _id,
                Name = Faker().Name,
                Description = Faker().Description,
                Price = Faker().Price,
                Thumb = ImageFake64(),
                CategoryId = Faker().CategoryId,
                IsActive = Faker().IsActive
            };
            
        }

        protected ProductModelOutPut ProductModelOutPut()
        {
            return new ProductModelOutPut
            ( 
                _id,
                Faker().Description,
                Faker().Name,
                Faker().Price,
                Faker().Thumb.Path,
                Faker().Stock,
                Faker().IsActive,
                Faker().CategoryId
            );

        }

        protected BusinessEntity.Product Faker()
        {
            BusinessEntity.Product product = (new BusinessEntity.Product
            (
                faker.Commerce.ProductName(),
                faker.Commerce.ProductDescription(),
                Convert.ToDecimal(faker.Commerce.Price()),
                _categoryId,
                faker.Random.UInt(),
                true
            ));

            product.UpdateThumb(faker.Image.LoremFlickrUrl());
            return product;
        }

        protected BusinessEntity.Category FakerCategory()
        {
            return new(faker.Commerce.Categories(1)[0]);
        }



        public static IEnumerable<object[]> GetUpdateProductInPutInvalid()
        {
            yield return new object[] { GetDescriptionProductGreaterThan1000CharactersInvalid() };
            yield return new object[] { GetDescriptionProductLessThan10CharactersInvalid() };
            yield return new object[] { GetNameProductGreaterThan255CharactersInvalid() };
            yield return new object[] { GetNameProductLessThan3CharactersInvalid() };
        }

        protected static ApplicationUseCase.UpdateProductInPut GetDescriptionProductGreaterThan1000CharactersInvalid()
        {
            string description = fakerStatic.Commerce.ProductDescription();
            while (description.Length < 1001)
            {
                description += fakerStatic.Commerce.ProductDescription();
            }


            return new ApplicationUseCase.UpdateProductInPut
            {
                Name = fakerStatic.Commerce.ProductName(),
                Description = description,
                Price = Convert.ToDecimal(fakerStatic.Commerce.Price()),
                Thumb = ImageFake64(),
                CategoryId = Guid.NewGuid(),
                IsActive = true
            };

        }

        protected static ApplicationUseCase.UpdateProductInPut GetDescriptionProductLessThan10CharactersInvalid()
        {
            string description = fakerStatic.Commerce.ProductDescription();
            description = description[..9];


            return new ApplicationUseCase.UpdateProductInPut
            {
                Name = fakerStatic.Commerce.ProductName(),
                Description = description,
                Price = Convert.ToDecimal(fakerStatic.Commerce.Price()),
                Thumb = ImageFake64(),
                CategoryId = Guid.NewGuid(),
                IsActive = true
            };

        }

        protected static ApplicationUseCase.UpdateProductInPut GetNameProductGreaterThan255CharactersInvalid()
        {
            string name = fakerStatic.Commerce.ProductName();
            while (name.Length < 255)
            {
                name += fakerStatic.Commerce.ProductName();
            }


            return new ApplicationUseCase.UpdateProductInPut
            {
                Name = name,
                Description = fakerStatic.Commerce.ProductDescription(),
                Price = Convert.ToDecimal(fakerStatic.Commerce.Price()),
                Thumb = ImageFake64(),
                CategoryId = Guid.NewGuid(),
                IsActive = true
            };

        }

        protected static ApplicationUseCase.UpdateProductInPut GetNameProductLessThan3CharactersInvalid()
        {
            string name = fakerStatic.Commerce.ProductDescription();
            name = name[..2];


            return new ApplicationUseCase.UpdateProductInPut
            {
                Name = name,
                Description = fakerStatic.Commerce.ProductDescription(),
                Price = Convert.ToDecimal(fakerStatic.Commerce.Price()),
                Thumb = ImageFake64(),
                CategoryId = Guid.NewGuid(),
                IsActive = true
            };

        }

    }
}
