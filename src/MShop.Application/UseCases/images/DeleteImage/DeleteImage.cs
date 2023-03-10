using MShop.Application.UseCases.images.Common;
using MShop.Business.Entity;
using MShop.Business.Interface;
using MShop.Business.Interface.Repository;
using MShop.Business.Interface.Service;


namespace MShop.Application.UseCases.images.DeleteImage
{
    public class DeleteImage : BaseUseCase, IDeleteImage
    {
        private readonly IImageRepository _imageRepository;
        private readonly IStorageService _storageService;
        public DeleteImage(IImageRepository imageRepository,
            IStorageService storageService,
            INotification notification) : base(notification)
        {
            _imageRepository = imageRepository;
            _storageService = storageService;   
        }

        public async Task<DeleteImageOutPutcs> Handler(DeleteImageInPut request)
        {
            var image = await _imageRepository.GetById(request.Id);

            if(image is null)
            {
                Notify("Não foi possivel encontrar a Image");
                throw new ApplicationException("");
            }

            if(await _storageService.Delete(image.FileName))
            {
                await _imageRepository.DeleteById(image);
            }

            return new DeleteImageOutPutcs(image.ProductId, new ImageModelOutPut(image.FileName));

        }
    }
}
