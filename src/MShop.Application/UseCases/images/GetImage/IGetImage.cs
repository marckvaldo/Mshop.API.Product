using MediatR;
using MShop.Application.UseCases.Images.Common;
using MShop.Application.UseCases.Images.GetImage;
using MShop.Core.DomainObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MShop.Application.UseCases.Images.GetImage
{
    public interface IGetImage : IRequestHandler<GetImageInPut, Result<ImageOutPut>>
    {
        Task<Result<ImageOutPut>> Handle(GetImageInPut request, CancellationToken cancellation);
    }
}
