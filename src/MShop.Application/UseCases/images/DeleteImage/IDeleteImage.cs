﻿using MediatR;
using MShop.Application.UseCases.Images.Common;
using MShop.Application.UseCases.Images.DeleteImage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MShop.Application.UseCases.Images.DeleteImage
{
    public interface IDeleteImage : IRequestHandler<DeleteImageInPut, ImageOutPut>
    {
        Task<ImageOutPut> Handle(DeleteImageInPut request, CancellationToken cancellationToken);
    }
}
