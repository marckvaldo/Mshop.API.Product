using MediatR;
using MShop.Application.UseCases.Images.Common;
using MShop.Application.UseCases.Images.ListImage;

namespace MShop.Application.UseCases.Images.ListImage
{
    public interface IListImage : IRequestHandler<ListImageInPut, ListImageOutPut>
    {
        Task<ListImageOutPut> Handle (ListImageInPut request, CancellationToken cancellation);
    }
}
