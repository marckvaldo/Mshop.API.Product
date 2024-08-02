using MediatR;
using MShop.Application.UseCases.Images.Common;
using MShop.Application.UseCases.Images.DeleteImage;
using MShop.Core.DomainObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MShop.Application.UseCases.Images.DeleteImage
{
    public interface IDeleteImage : IRequestHandler<DeleteImageInPut, Result<ImageOutPut>>
    {
        Task<Result<ImageOutPut>> Handle(DeleteImageInPut request, CancellationToken cancellationToken);
    }
}
