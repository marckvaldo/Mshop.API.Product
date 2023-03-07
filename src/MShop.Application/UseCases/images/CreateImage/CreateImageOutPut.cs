using MShop.Application.UseCases.images.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MShop.Application.UseCases.images.CreateImage
{
    public class CreateImageOutPut
    {
        public CreateImageOutPut(Guid productId, List<ImageModelOutPut> image)
        {
            ProductId = productId;
            Image = image;
        }

        public Guid ProductId { get; set; }

        public List<ImageModelOutPut> Image { get; set; }
    }
}
