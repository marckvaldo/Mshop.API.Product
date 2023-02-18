using BusinessEntity = MShop.Business.Entity;
using UseCase = MShop.Application.UseCases.Product;
using MShop.EndToEndTest.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MShop.Application.UseCases.Product.CreateProducts;
using MShop.EndToEndTest.API.Category;

namespace MShop.EndToEndTest.API.Product
{
    public class ProductAPITestFixture : BaseFixture
    {
        private Guid _categoryId;
        private readonly Guid _id;

        protected readonly ProductPersistence Persistence;
        protected readonly CategoryPersistence CategoryPersistence;
        protected readonly ProductPersistenceCache ProductPersistenceCache;

        public ProductAPITestFixture() : base()
        {
            //_categoryId = Guid.NewGuid();
            _id = Guid.NewGuid();

            Persistence = new ProductPersistence(
                CreateDBContext()
            );

            CategoryPersistence = new CategoryPersistence(
                CreateDBContext()
            );


            ProductPersistenceCache = new ProductPersistenceCache(
                CreateCache()
            );
        }

        protected async Task<BusinessEntity.Product> Faker()
        {
            await BuildCategory();
           var product = (new BusinessEntity.Product
            (
                faker.Commerce.ProductDescription(),
                faker.Commerce.ProductName(),
                Convert.ToDecimal(faker.Commerce.Price()),
                faker.Image.LoremPixelUrl(),
                _categoryId,
                faker.Random.UInt(),
                true
            ));
            return product;
        }

        public async Task<CreateProductInPut> RequestCreate()
        {
            var faker = await Faker();
            return new CreateProductInPut
            {
                Name = faker.Name,
                CategoryId = _categoryId,
                Imagem = faker.Imagem,
                IsActive = true,
                Description = faker.Description,
                Price = faker.Price,
                Stock = faker.Stock
            };
        }

        public async Task<UseCase.UpdateProduct.UpdateProductInPut> RequestUpdate()
        {
            var faker = await Faker();
            return new UseCase.UpdateProduct.UpdateProductInPut
            {
                Name = faker.Name,
                CategoryId = _categoryId,
                Imagem = faker.Imagem,
                IsActive = true,
                Description = faker.Description,
                Price = faker.Price,
                Id = _id
            };
        }

        public async Task<List<BusinessEntity.Product>> GetProducts(int length = 10)
        {
            List<BusinessEntity.Product> products = new List<BusinessEntity.Product>();
            for (int i = 0; i < length; i++)
                products.Add(await Faker());

            return products;

        }

        public string GetDescriptionProductGreaterThan1000CharactersInvalid()
        {
            string description = fakerStatic.Commerce.ProductDescription();
            while (description.Length < 1001)
            {
                description += fakerStatic.Commerce.ProductDescription();
            }

            return description;
        }

        public string GetNameProductGreaterThan255CharactersInvalid()
        {
            string description = fakerStatic.Commerce.ProductName();
            while (description.Length < 256)
            {
                description += fakerStatic.Commerce.ProductName();
            }

            return description;
        }

        protected async Task BuildCategory()
        {
            var nameCategory = faker.Commerce.Categories(1)[0].ToString();

            await CategoryPersistence.Create(new BusinessEntity.Category(nameCategory, true));
            var category = await CategoryPersistence.GetByIdName(nameCategory);
            if(category is not null)
                _categoryId = category.Id;
        }
    }
}
