using MShop.Application.Common;
using MShop.Application.UseCases.images.CreateImage;
using MShop.IntegrationTests.Application.UseCase.Images.Commom;
using MShop.IntegrationTests.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessModel = MShop.Business.Entity;

namespace MShop.IntegrationTests.Application.UseCase.Images.CreateImage
{
    public class CreateImageTestFixture : ImageTestFixture
    {
        public CreateImageInPut FakerRequest(Guid productId, int quantity = 3) 
        {
            return new CreateImageInPut
            {
                Images = FakeFileInputList64(quantity),
                ProductId = productId
            }; 
        }

        public BusinessModel.Category FakerCategory()
        {
            return new(faker.Commerce.Categories(1)[0]);
        }

        protected BusinessModel.Product FakerProduct(BusinessModel.Category category)
        {
            var produto = new BusinessModel.Product(faker.Commerce.ProductDescription(),
                                    faker.Commerce.ProductName(),
                                    Convert.ToDecimal(faker.Commerce.Price()),
                                    category.Id,
                                    faker.Random.UInt(),
                                    true
                                );
            produto.UpdateCategory(category);
            return produto;
        }
    }
}
