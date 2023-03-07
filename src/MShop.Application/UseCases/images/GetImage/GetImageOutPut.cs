using MShop.Application.UseCases.images.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MShop.Application.UseCases.images.GetImage
{
    public class GetImageOutPut
    {
        public GetImageOutPut(Guid productId, ImageModelOutPut image)
        {
            ProductId = productId;
            Image = image;
        }

        public Guid ProductId { get; set; }

        public ImageModelOutPut Image { get; set; }
    }
}
