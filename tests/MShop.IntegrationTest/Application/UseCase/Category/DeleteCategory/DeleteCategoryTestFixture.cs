using MShop.Application.UseCases.Category.CreateCategory;
using MShop.Business.Entity;
using MShop.IntegrationTests.Application.UseCase.Category.Common;
using MShop.IntegrationTests.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessEntity = MShop.Business.Entity;

namespace MShop.IntegrationTests.Application.UseCase.Category.DeleteCategory
{
    public class DeleteCategoryTestFixture : CategoryTestFixture
    {

        public BusinessEntity.Product FakerProduct(Guid categoryId)
        {
            return new BusinessEntity.Product
                (
                    faker.Commerce.ProductDescription(),
                    faker.Commerce.ProductName(),
                    decimal.Parse(faker.Commerce.Price()),
                    categoryId,
                    0,
                    true
                );
        }

        public List<BusinessEntity.Product> FakerProducts(Guid categoryId, int quantity)
        {
            List<BusinessEntity.Product> listProduct = new();
            for(int i = 0; i<quantity;i++)
                listProduct.Add(FakerProduct(categoryId)); 

            return listProduct;
        }      

    }
}
