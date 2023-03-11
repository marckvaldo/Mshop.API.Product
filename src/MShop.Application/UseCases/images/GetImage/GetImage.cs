using MShop.Application.UseCases.images.Common;
using MShop.Business.Interface;
using MShop.Business.Interface.Repository;
using MShop.Business.Interface.Service;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MShop.Application.UseCases.images.GetImage
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

        public async Task<GetImageOutPut> Handler(GetImageInPut request)
        {
            var image = await _imageRepository.GetById(request.id);

            if (image is null)
            {
                Notify("Não foi possivel localizar image na base de dados!");
                throw new ApplicationException("");
            }

            return new GetImageOutPut(image.ProductId, new ImageModelOutPut(image.FileName));
        }
    }
}
