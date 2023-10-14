using MShop.Application.UseCases.Images.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MShop.Application.UseCases.Images.Common
{
    public class ListImageOutPut
    {
        public ListImageOutPut(Guid productId, List<ImageModelOutPut> images)
        {
            ProductId = productId;
            Images = images;
        }

        public Guid ProductId { get; set; }

        public List<ImageModelOutPut> Images { get; set; }
    }
}
