﻿using MShop.Application.UseCases.images.Common;
using MShop.Business.Interface;
using MShop.Business.Interface.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MShop.Application.UseCases.images.ListImage
{
    public class ListImage : BaseUseCase, IListImage
    {
        private readonly IImageRepository _imageRepository;
        public ListImage(INotification notification, IImageRepository imageRepository) : base(notification)
        {
            _imageRepository = imageRepository;
        }

        public async Task<ListImageOutPut> Handler(Guid productId)
        {
            var images = await _imageRepository.Filter(x=>x.ProductId == productId);

            return new ListImageOutPut
                (
                    productId, 
                    images.Select(x => new ImageModelOutPut(x.FileName)).ToList()
                ) ;
        }
    }
}
