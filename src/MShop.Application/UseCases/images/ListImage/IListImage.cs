using MediatR;
using MShop.Application.UseCases.Images.Common;
using MShop.Application.UseCases.Images.ListImage;
using MShop.Core.DomainObject;

namespace MShop.Application.UseCases.Images.ListImage
{
    public interface IListImage : IRequestHandler<ListImageInPut, Result<ListImageOutPut>>
    {
        Task<Result<ListImageOutPut>> Handle (ListImageInPut request, CancellationToken cancellation);
    }
}
