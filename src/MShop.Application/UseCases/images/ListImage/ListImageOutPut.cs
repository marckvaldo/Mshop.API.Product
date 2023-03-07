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
        public ListImageOutPut(Guid productId, List<ImageModelOutPut> productName)
        {
            ProductId = productId;
            ProductName = productName;
        }

        public Guid ProductId { get; set; }

        public List<ImageModelOutPut> ProductName { get; set; }   
    }
}
