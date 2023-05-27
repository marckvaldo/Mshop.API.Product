﻿using MShop.Application.UseCases.images.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MShop.Application.UseCases.images.CreateImage
{
    public interface ICreateImage
    {
        Task<ListImageOutPut> Handler(CreateImageInPut request, CancellationToken cancellation);
    }
}
