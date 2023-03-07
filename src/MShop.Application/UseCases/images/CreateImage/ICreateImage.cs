using MShop.Application.UseCases.images.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MShop.Application.UseCases.images.CreateImage
{
    internal interface ICreateImage
    {
        Task<CreateImageOutPut> Handler(CreateImageInPut request);
    }
}
