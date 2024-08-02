using MShop.Application.UseCases.Images.Common;
using MShop.Core.Base;
using MShop.Core.DomainObject;
using MShop.Core.Message;
using MShop.Repository.Interface;

namespace MShop.Application.UseCases.Images.ListImage
{
    public class ListImage : BaseUseCase, IListImage
    {
        private readonly IImageRepository _imageRepository;
        public ListImage(INotification notification, IImageRepository imageRepository) : base(notification)
            => _imageRepository = imageRepository;
        public async Task<Result<ListImageOutPut>> Handle(ListImageInPut request, CancellationToken cancellation)
        {
            var images = await _imageRepository.Filter(x=>x.ProductId == request.Id);
            var imagesOutPut = new ListImageOutPut
                    (
                        request.Id, 
                        images.Select(x => new ImageModelOutPut(x.FileName)).ToList()
                    );

            return Result<ListImageOutPut>.Success(imagesOutPut);
        }
    }
}
