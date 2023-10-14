using MShop.Application.UseCases.Images.Common;
using MShop.Application.UseCases.Images.ListImage;
using MShop.Business.Interface;
using MShop.Business.Interface.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MShop.Application.UseCases.Images.ListImage
{
    public class ListImage : BaseUseCase, IListImage
    {
        private readonly IImageRepository _imageRepository;
        public ListImage(INotification notification, IImageRepository imageRepository) : base(notification)
            => _imageRepository = imageRepository;
        public async Task<ListImageOutPut> Handle(ListImageInPut request, CancellationToken cancellation)
        {
            var images = await _imageRepository.Filter(x=>x.ProductId == request.Id);
            return new ListImageOutPut
                    (
                        request.Id, 
                        images.Select(x => new ImageModelOutPut(x.FileName)).ToList()
                    );
        }
    }
}
