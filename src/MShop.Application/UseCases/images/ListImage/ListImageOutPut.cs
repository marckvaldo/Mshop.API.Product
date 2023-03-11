using MShop.Application.UseCases.images.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MShop.Application.UseCases.images.ListImage
{
    public class ListImageOutPut
    {
        public ListImageOutPut(Guid productId, List<ImageModelOutPut> imageName)
        {
            ProductId = productId;
            ImageName = imageName;
        }

        public Guid ProductId { get; set; }

        public List<ImageModelOutPut> ImageName { get; set; }   
    }
}
