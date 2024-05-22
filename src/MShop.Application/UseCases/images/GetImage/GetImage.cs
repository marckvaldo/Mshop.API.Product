using MShop.Application.UseCases.Images.Common;
using MShop.Business.Interface.Service;
using MShop.Core.Base;
using MShop.Core.Message;
using MShop.Repository.Interface;

namespace MShop.Application.UseCases.Images.GetImage
{
    public class GetImage : BaseUseCase, IGetImage
    {
        private readonly IImageRepository _imageRepository;
        private readonly IStorageService _storageService;
        public GetImage(INotification notification, 
            IImageRepository imageRepository, 
            IStorageService storageService) : base(notification)
        {
            _storageService = storageService;   
            _imageRepository = imageRepository; 
        }

        public async Task<ImageOutPut> Handle(GetImageInPut request, CancellationToken cancellation)
        {
            var image = await _imageRepository.GetById(request.Id);            
            NotifyExceptionIfNull(image, "Não foi possivel localizar image na base de dados!");

            return new ImageOutPut(image!.ProductId, new ImageModelOutPut(image.FileName));
        }
    }
}
