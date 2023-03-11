using Mshop.Test.Common;
using MShop.Application.Common;
using MShop.Application.UseCases.images.CreateImage;
using MShop.UnitTests.Application.UseCases.Image.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MShop.UnitTests.Application.UseCases.Image.CreateImage
{
    public class CreateImageTestFixture : ImageBaseFixtureTest
    {
        public CreateImageInPut FakerRequest(Guid productId, List<FileInput> images)
        {
            return new CreateImageInPut
            {
                Images = images,
                ProductId = productId
            };
        }

        
    }
}
