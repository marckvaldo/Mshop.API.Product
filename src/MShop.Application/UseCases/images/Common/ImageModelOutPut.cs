using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MShop.Application.UseCases.Images.Common
{
    public class ImageModelOutPut
    {
        public ImageModelOutPut(string image)
        {
            Image = image;
        }

        public string Image { get; set; }
    }
}
