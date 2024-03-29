﻿using Mshop.Test.Common;
using MShop.Application.Common;
using MShop.Application.UseCases.Images.CreateImage;
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
        public CreateImageInPut FakerRequest(Guid productId, List<FileInputBase64> images)
        {
            return new CreateImageInPut
            {
                Images = images,
                ProductId = productId
            };
        }

        

        
    }
}
