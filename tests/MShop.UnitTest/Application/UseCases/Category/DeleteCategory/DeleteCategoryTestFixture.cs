using BusinessEntity = MShop.Business.Entity;
using MShop.UnitTests.Application.UseCases.Category.common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace MShop.UnitTests.Application.UseCases.Category.DeleteCategory
{
    public class DeleteCategoryTestFixture : CategoryBaseFixtureTest
    {
        public BusinessEntity.Product FakerProduct(Guid categoryId)
        {
            return new BusinessEntity.Product(
                faker.Commerce.ProductDescription(), 
                faker.Commerce.ProductName(), 
                decimal.Parse(faker.Commerce.Price()),
                categoryId == Guid.Empty ? new Guid() : categoryId, 
                faker.Random.Int(), 
                true);
        }

        public List<BusinessEntity.Product> FakerProducts(int quantity,Guid categoryId)
        {
            List<BusinessEntity.Product> listProduct = new List<BusinessEntity.Product>();
            for (int i = 1; i <= quantity; i++)
                listProduct.Add(FakerProduct(categoryId));

            return listProduct;
        }
    }
}
