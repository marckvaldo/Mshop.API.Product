using MediatR;
using MShop.Application.UseCases.Images.Common;

namespace MShop.Application.UseCases.Images.CreateImage
{
    public interface ICreateImage : IRequestHandler<CreateImageInPut, ListImageOutPut>
    {
        Task<ListImageOutPut> Handle(CreateImageInPut request, CancellationToken cancellation);
    }
}
