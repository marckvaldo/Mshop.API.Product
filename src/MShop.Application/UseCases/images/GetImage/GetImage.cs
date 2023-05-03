using MShop.Application.UseCases.images.Common;
using MShop.Business.Exception;
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

        public async Task<ImageOutPut> Handler(Guid id)
        {
            var image = await _imageRepository.GetById(id);

            NotFoundException.ThrowIfnull(image, "Não foi possivel localizar image na base de dados!");

            return new ImageOutPut(image.ProductId, new ImageModelOutPut(image.FileName));
        }
    }
}
