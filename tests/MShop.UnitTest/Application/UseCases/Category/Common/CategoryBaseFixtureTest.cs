using Mshop.Test.Common;
using MShop.Application.UseCases.Category.CreateCategory;
using MShop.UnitTests.Common;
using BusinessEntity = MShop.Business.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MShop.UnitTests.Application.UseCases.Category.common
{
    public class CategoryBaseFixtureTest : BaseFixture
    {
       

        public BusinessEntity.Category Faker()
        {
            return new BusinessEntity.Category(faker.Commerce.Categories(1)[0]);
        }

        public List<BusinessEntity.Category> FakerCategorys(int quantity)
        {
            List<BusinessEntity.Category> listCategory = new List<BusinessEntity.Category>();
            for (int i = 1; i <= quantity; i++)
                listCategory.Add(Faker());

            return listCategory;
        }


        public static IEnumerable<object[]> ListNamesCategoryInvalid()
        {
            yield return new object[] { InvalidData.GetNameCategoryGreaterThan30CharactersInvalid() };
            yield return new object[] { InvalidData.GetNameCategoryLessThan3CharactersInvalid() };
            yield return new object[] { "" };
            yield return new object[] { null };
        }

        public BusinessEntity.Product FakerProduct(BusinessEntity.Category category)
        {
            var product = new BusinessEntity.Product
                (
                     faker.Commerce.ProductName(),
                     faker.Commerce.ProductDescription(),
                    Convert.ToDecimal(faker.Commerce.Price()),
                    category.Id,
                    faker.Random.UInt(),
                    true
                );

            return product;
        }

        public BusinessEntity.Category FakerCategory()
        {
            var category = new BusinessEntity.Category
                (
                     faker.Commerce.Categories(1)[0],
                     true
                );

            return category;
        }

        public List<BusinessEntity.Product> FakerProducts(int quantity, BusinessEntity.Category category)
        {
            List<BusinessEntity.Product> listProduct = new List<BusinessEntity.Product>();
            for (int i = 1; i <= quantity; i++)
                listProduct.Add(FakerProduct(category));

            return listProduct;
        }
    }
}
