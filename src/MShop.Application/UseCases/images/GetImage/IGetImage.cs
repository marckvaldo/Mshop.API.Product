using MShop.Application.UseCases.images.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MShop.Application.UseCases.images.GetImage
{
    internal interface IGetImage
    {
        Task<GetImageOutPut> Handler(GetImageInPut request);
    }
}
