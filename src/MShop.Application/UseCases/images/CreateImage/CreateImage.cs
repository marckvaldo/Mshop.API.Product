using MShop.Application.Common;
using MShop.Application.UseCases.images.Common;
using MShop.Business.Entity;
using MShop.Business.Interface;
using MShop.Business.Interface.Repository;
using MShop.Business.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MShop.Application.UseCases.images.CreateImage
{
    public class CreateImage : BaseUseCase,  ICreateImage
    {
        private readonly IImageRepository _imageRepository;
        private readonly IStorageService _storageService;
        public CreateImage(IImageRepository imageRepository,
            IStorageService storageService,
            INotification notification) : base(notification)
        {
            _imageRepository = imageRepository;
            _storageService = storageService;
        }

        public async Task<CreateImageOutPut> Handler(CreateImageInPut request)
        {
            if (request.Images is null)
            {
                Notify("Infome um container de imagens!");
                throw new ApplicationException("Infome um container de imagens!");
            }


            List<Image> Images = new();

            foreach (FileInput item in request.Images)
            {
                var image = new Image("", request.ProductId);

                if(item is not null)
                {
                    var urlImage  = await  _storageService.Upload($"image-{image.Id}-{image.ProductId}.{item.Extension}", item.FileStrem);
                    image.UpdateUrlImage(urlImage);
                    image.IsValid(_notifications);
                    Images.Add(image);
                }
            }

            if(Images.Count == 0)
            {
                Notify("Não foi possível salvar as images");
                throw new ApplicationException("Não foi possível salvar as images");
            }

            await _imageRepository.CreateRange(Images);

            return new CreateImageOutPut(request.ProductId, Images.Select(x=> new ImageModelOutPut(x.FileName)).ToList());
        }
    }
}
