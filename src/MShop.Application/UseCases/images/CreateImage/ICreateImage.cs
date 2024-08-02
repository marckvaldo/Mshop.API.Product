using MediatR;
using MShop.Application.UseCases.Images.Common;
using MShop.Core.DomainObject;

namespace MShop.Application.UseCases.Images.CreateImage
{
    public interface ICreateImage : IRequestHandler<CreateImageInPut, Result<ListImageOutPut>>
    {
        Task<Result<ListImageOutPut>> Handle(CreateImageInPut request, CancellationToken cancellation);
    }
}
