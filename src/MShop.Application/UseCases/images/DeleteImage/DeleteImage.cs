using MShop.Application.UseCases.Images.Common;
using MShop.Business.Interface.Service;
using MShop.Core.Base;
using MShop.Core.Data;
using MShop.Core.DomainObject;
using MShop.Core.Message;
using MShop.Repository.Interface;


namespace MShop.Application.UseCases.Images.DeleteImage
{
    public class DeleteImage : BaseUseCase, IDeleteImage
    {
        private readonly IImageRepository _imageRepository;
        private readonly IStorageService _storageService;
        private readonly IUnitOfWork _unitOfWork;
        public DeleteImage(IImageRepository imageRepository,
            IStorageService storageService,
            INotification notification,
            IUnitOfWork unitOfWork) : base(notification)
        {
            _imageRepository = imageRepository;
            _storageService = storageService;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<ImageOutPut>> Handle(DeleteImageInPut request, CancellationToken cancellationToken)
        {
            var image = await _imageRepository.GetById(request.Id);
            //NotifyExceptionIfNull(image, "Não foi possivel encontrar a Image");
            if (NotifyErrorIfNull(image, "Não foi possivel encontrar a Image"))
                return Result<ImageOutPut>.Error();
                       
            if(await _storageService.Delete(image!.FileName))
            {
                await _imageRepository.DeleteById(image, cancellationToken);
                await _unitOfWork.CommitAsync(cancellationToken);
            }

            var imageOutPut = new ImageOutPut(image.ProductId, new ImageModelOutPut(image.FileName));
            return Result<ImageOutPut>.Success(imageOutPut);
            
        }

    }
}
