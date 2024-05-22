using MShop.Application.Common;
using MShop.Application.UseCases.Images.Common;
using MShop.Business.Entity;
using MShop.Business.Interface.Service;
using MShop.Core.Data;
using MShop.Core.Message;
using MShop.Repository.Interface;


namespace MShop.Application.UseCases.Images.CreateImage
{
    public class CreateImage : Core.Base.BaseUseCase,  ICreateImage
    {
        private readonly IImageRepository _imageRepository;
        private readonly IProductRepository _productRepository;
        private readonly IStorageService _storageService;
        private readonly IUnitOfWork _unitOfWork;
        public CreateImage(IImageRepository imageRepository,
            IStorageService storageService,
            IProductRepository productRepository,
            INotification notification,
            IUnitOfWork unitOfWork) : base(notification)
        {
            _imageRepository = imageRepository;
            _productRepository = productRepository;
            _storageService = storageService;
            _unitOfWork = unitOfWork;
        }

        public async Task<ListImageOutPut> Handle(CreateImageInPut request, CancellationToken cancellationToken)
        {
            var hasProduct = await _productRepository.GetById(request.ProductId);            
            NotifyExceptionIfNull(hasProduct, "Não foi possivel localizar produtos informado");

            if (request.Images?.Count == 0)
                NotifyException("Por favor informar ao menos uma image");

            List<Image> Images = new();
            foreach (FileInputBase64 item in request.Images)
            {
                if (string.IsNullOrEmpty(item.FileStremBase64.Trim()))
                    continue;

                var image = new Image("", request.ProductId);
                if(item is not null)
                {
                    var file = Helpers.Base64ToStream(item.FileStremBase64);
                    var urlImage  = await  _storageService.Upload($"image-{image.Id}-{image.ProductId}.{file.Extension}", file.FileStrem);
                    image.UpdateUrlImage(urlImage);
                    image.IsValid(Notifications);
                    Images.Add(image);
                }
            }

            if (Images.Count == 0)
                NotifyException("Não foi possível salvar as Images");
           
            await _imageRepository.CreateRange(Images, cancellationToken);
            await _unitOfWork.CommitAsync(cancellationToken);

            return new ListImageOutPut(request.ProductId, Images.Select(x=> new ImageModelOutPut(x.FileName)).ToList());
        }

        
    }
}
