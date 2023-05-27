using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MShop.Business.Interface;
using MShop.Application.UseCases.images.Common;
using MShop.Application.UseCases.images.GetImage;
using MShop.Application.UseCases.images.DeleteImage;
using MShop.Application.UseCases.images.CreateImage;
using MShop.Application.UseCases.images.ListImage;
using MShop.Application.UseCases.Product.Common;

namespace MShop.ProductAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageController : MainController
    {

        private readonly IGetImage _getImage;
        private readonly IDeleteImage _deleteImage;
        private readonly ICreateImage _createImage;
        private readonly IListImage _listImage;

        public ImageController(
            IGetImage getImage,
            IDeleteImage deleteImage,
            ICreateImage createImage,
            IListImage listImage,
            INotification notification
            ) : base(notification)
        {
            _getImage = getImage;
            _deleteImage = deleteImage;
            _createImage = createImage;
            _listImage = listImage;
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<ImageOutPut>> Image(Guid id)
        {
            return CustomResponse(await _getImage.Handler(id));        
        }

        [HttpGet("list-images-by-id-production/{productId:guid}")]
        public async Task<ActionResult<ListImageOutPut>> ListImagesByIdProduction(Guid productId)
        {
            return CustomResponse(await _listImage.Handler(productId));
        }

        [HttpPost]        
        public async Task<ActionResult<ListImageOutPut>> Create(CreateImageInPut image, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid) CustomResponse(ModelState);
            return CustomResponse(await _createImage.Handler(image, cancellationToken));
        }

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<ImageOutPut>> Delete(Guid id, CancellationToken cancellationToken)
        {
            return CustomResponse(await _deleteImage.Handler(id, cancellationToken));
        }

    }
}

