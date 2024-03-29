﻿using MediatR;
using MShop.Application.UseCases.Images.Common;
using MShop.Application.UseCases.Images.GetImage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MShop.Application.UseCases.Images.GetImage
{
    public interface IGetImage : IRequestHandler<GetImageInPut, ImageOutPut>
    {
        Task<ImageOutPut> Handle(GetImageInPut request, CancellationToken cancellation);
    }
}
