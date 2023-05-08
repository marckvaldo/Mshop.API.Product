using MShop.IntegrationTests.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessEntity = MShop.Business.Entity;
using MShop.Business.ValueObject;
using MShop.IntegrationTests.Application.UseCase.Product.Common;
using MShop.Business.Entity;

namespace MShop.IntegrationTests.Application.UseCase.Product.ListProduct
{
    public class ListProductTestFixture : ProductTestFixture
    {
       
        protected List<BusinessEntity.Product> ListFake(int lengt = 5)
        {
            List<BusinessEntity.Product> listProducts = new List<BusinessEntity.Product>();

            for (int i = 0; i < lengt; i++)
                listProducts.Add(Faker(Guid.NewGuid()));

            return listProducts;
        }

        protected List<BusinessEntity.Product> ListFake(BusinessEntity.Category category, int lengt = 5)
        {
            List<BusinessEntity.Product> listProducts = new List<BusinessEntity.Product>();

            for (int i = 0; i < lengt; i++)
                listProducts.Add(Faker(Guid.NewGuid(),category));

            return listProducts;
        }
    }
}
