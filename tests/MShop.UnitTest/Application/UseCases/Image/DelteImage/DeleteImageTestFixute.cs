using MShop.Application.UseCases.images.DeleteImage;
using MShop.UnitTests.Application.UseCases.Image.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MShop.UnitTests.Application.UseCases.Image.DelteImage
{
    public class DeleteImageTestFixute : ImageBaseFixtureTest
    {
        public DeleteImageInPut FakerRequest()
        {
            return new DeleteImageInPut
            {
                Id = Guid.NewGuid(),
            };
        }
    }
}
