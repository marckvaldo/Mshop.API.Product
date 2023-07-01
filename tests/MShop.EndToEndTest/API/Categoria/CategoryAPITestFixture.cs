using BusinessEntity = MShop.Business.Entity;
using UseCase = MShop.Application.UseCases.Product;
using MShop.EndToEndTest.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MShop.Application.UseCases.Category.CreateCategory;
using MShop.Application.UseCases.Category.UpdateCategory;

namespace MShop.EndToEndTest.API.Categoria
{
    public class CategoryAPITestFixture : BaseFixture
    {
        private readonly Guid _id;

        public CategoryPersistence Persistence;

        public CategoryAPITestFixture() : base()
        {
            _id = Guid.NewGuid();

            Persistence = new CategoryPersistence(
                CreateDBContext()
            );
        }

        protected BusinessEntity.Category Faker()
        {
            string name = faker.Commerce.ProductName();
            var product = new BusinessEntity.Category
            (
                (name.Length > 30 ? name[..29] : name),
                true
            );
            return product;
        }

        public CreateCategoryInPut RequestCreate()
        {
            return new CreateCategoryInPut
            {
                Name = Faker().Name,
            };
        }

        public UpdateCategoryInPut RequestUpdate()
        {
            return new UpdateCategoryInPut
            {
                Name = Faker().Name,
                Id = _id
            };
        }


        public List<BusinessEntity.Category> GetCategory(int length = 10)
        {
            List<BusinessEntity.Category> category = new List<BusinessEntity.Category>();
            for (int i = 0; i < length; i++)
                category.Add(Faker());

            return category;

        }

        public string GetDescriptionCategoryGreaterThan1000CharactersInvalid()
        {
            string description = fakerStatic.Commerce.ProductDescription();
            while (description.Length < 1001)
            {
                description += fakerStatic.Commerce.ProductDescription();
            }

            return description;
        }

        public string GetNameCategoryGreaterThan255CharactersInvalid()
        {
            string description = fakerStatic.Commerce.ProductName();
            while (description.Length < 256)
            {
                description += fakerStatic.Commerce.ProductName();
            }

            return description;
        }

    }
}
