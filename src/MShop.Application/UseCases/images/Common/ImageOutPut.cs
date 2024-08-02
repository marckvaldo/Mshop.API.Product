using MShop.Core.DomainObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MShop.Application.UseCases.Images.Common
{
    public class ImageOutPut: IModelOutPut
    {
        public ImageOutPut(Guid productId, ImageModelOutPut image)
        {
            ProductId = productId;
            Image = image;
        }

        public Guid ProductId { get; set; }

        public ImageModelOutPut Image { get; set; }

    }
}
