using MShop.IntegrationTests.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessEntity = MShop.Business.Entity;
using ApplicationUseCase = MShop.Application.UseCases.Product.UpdateProduct;
using MShop.Business.ValueObject;
using MShop.Application.Common;
using MShop.IntegrationTests.Application.UseCase.Product.Common;

namespace MShop.IntegrationTests.Application.UseCase.Product.UpdateProduct
{
    public class UpdateProdutTestFixture : ProductTestFixture
    {
        

        protected ApplicationUseCase.UpdateProductInPut RequestFake()
        {
            var product = (new ApplicationUseCase.UpdateProductInPut
            {
                Description = Faker().Description,
                Name = Faker().Name,
                Price = Convert.ToDecimal(Faker().Price),
                Thumb = ImageFake64(),
                CategoryId = _categoryId,
                IsActive = true,
                Id = _id    
            });
            return product;
        }
    }
}
