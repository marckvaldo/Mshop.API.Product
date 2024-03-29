﻿using MShop.Application.Common;
using MShop.Business.Entity;
using MShop.IntegrationTests.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessEntity = MShop.Business.Entity;

namespace MShop.IntegrationTests.Application.UseCase.Product.Common
{
    public class ProductTestFixture : BaseFixture
    {
        protected readonly Guid _categoryId;
        protected readonly Guid _id;
        protected readonly BusinessEntity.Category _category;
        public ProductTestFixture() : base()
        {
            _categoryId = Guid.NewGuid();
            _id = Guid.NewGuid();
            _category = null;
        }

        protected static FileInput ImageFake()
        {
            return new FileInput("jpg", new MemoryStream(Encoding.ASCII.GetBytes(fakerStatic.Image.LoremPixelUrl())));
        }

        protected BusinessEntity.Category FakeCategory()
        {
            return new(faker.Commerce.Categories(1)[0]);
        }

        protected BusinessEntity.Product Faker()
        {
            var product = (new BusinessEntity.Product
            (
                faker.Commerce.ProductName(),
                faker.Commerce.ProductDescription(),
                Convert.ToDecimal(faker.Commerce.Price()),
                _categoryId,
                faker.Random.UInt(),
                true
            ));
            product.Id = _id;
            return product;
        }

        protected BusinessEntity.Product Faker(Guid productId)
        {
            var product = (new BusinessEntity.Product
            (
                faker.Commerce.ProductName(),
                faker.Commerce.ProductDescription(),
                Convert.ToDecimal(faker.Commerce.Price()),
                _categoryId,
                faker.Random.UInt(),
                true
            ));
            product.Id = productId;
            return product;
        }

        protected BusinessEntity.Product Faker(BusinessEntity.Category category)
        {
            var product = (new BusinessEntity.Product
            (
                faker.Commerce.ProductName(),
                faker.Commerce.ProductDescription(),
                Convert.ToDecimal(faker.Commerce.Price()),
                category.Id,
                faker.Random.UInt(),
                true
            ));
            product.UpdateCategory(category);
            return product;
        }

        protected BusinessEntity.Product Faker(Guid productId, BusinessEntity.Category category)
        {
            var product = (new BusinessEntity.Product
            (
                faker.Commerce.ProductName(),
                faker.Commerce.ProductDescription(),
                Convert.ToDecimal(faker.Commerce.Price()),
                category.Id,
                faker.Random.UInt(),
                true
            ));
            product.Id = productId;
            product.UpdateCategory(category);
            return product;
        }

        protected BusinessEntity.Category FakeCategory(Guid categoryId)
        {
            var category = new BusinessEntity.Category(faker.Commerce.Categories(1)[0], true);
            category.Id = categoryId;
            return category;
        }
    }
}
