using MShop.Application.Common;
using MShop.Application.UseCases.images.CreateImage;
using MShop.IntegrationTests.Application.UseCase.Images.Commom;
using MShop.IntegrationTests.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
