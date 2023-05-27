﻿using MShop.Application.UseCases.images.Common;
using MShop.Application.UseCases.images.CreateImage;
using MShop.Business.Entity;
using MShop.Business.Exception;
using MShop.Business.Interface;
using MShop.Business.Interface.Repository;
using MShop.Business.Interface.Service;
using static System.Net.Mime.MediaTypeNames;


namespace MShop.Application.UseCases.images.DeleteImage
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

        public async Task<ImageOutPut> Handler(Guid id, CancellationToken cancellationToken)
        {
            var image = await _imageRepository.GetById(id);
            NotifyExceptionIfNull(image, "Não foi possivel encontrar a Image");

            if(await _storageService.Delete(image!.FileName))
            {
                await _imageRepository.DeleteById(image, cancellationToken);
                await _unitOfWork.CommitAsync(cancellationToken);
            }

            return new ImageOutPut(image.ProductId, new ImageModelOutPut(image.FileName));
            
        }
    }
}
