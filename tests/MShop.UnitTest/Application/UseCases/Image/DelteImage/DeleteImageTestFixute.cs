﻿using MShop.Application.UseCases.Images.Common;
using MShop.Application.UseCases.Images.DeleteImage;
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
        public Guid FakerRequest()
        {
            return Guid.NewGuid();
        }
    }
}
