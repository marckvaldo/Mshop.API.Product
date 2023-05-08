using MShop.Application.Common;
using MShop.Application.UseCases.images.Common;
using MShop.Business.Entity;
using MShop.Business.Exception;
using MShop.Business.Interface;
using MShop.Business.Interface.Repository;
using MShop.Business.Interface.Service;
using MShop.Repository.Repository;


namespace MShop.Application.UseCases.images.CreateImage
{
    public class CreateImage : BaseUseCase,  ICreateImage
    {
        private readonly IImageRepository _imageRepository;
        private readonly IProductRepository _productRepository;
        private readonly IStorageService _storageService;
        public CreateImage(IImageRepository imageRepository,
            IStorageService storageService,
            IProductRepository productRepository,
            INotification notification) : base(notification)
        {
            _imageRepository = imageRepository;
            _productRepository = productRepository;
            _storageService = storageService;
        }

        public async Task<ListImageOutPut> Handler(CreateImageInPut request)
        {

           
            var hasProduct = await _productRepository.GetById(request.ProductId);
            //NotFoundException.ThrowIfnull(hasProduct, "Não foi possivel localizar produtos informado");
            NotifyExceptionIfNull(hasProduct, "Não foi possivel localizar produtos informado");

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
                NotifyException("Não foi possível salvar as images");
            //{
                //Notify("Não foi possível salvar as images");
                //throw new ApplicationException("Não foi possível salvar as images");
            //}

            await _imageRepository.CreateRange(Images);

            return new ListImageOutPut(request.ProductId, Images.Select(x=> new ImageModelOutPut(x.FileName)).ToList());
        }
    }
}
