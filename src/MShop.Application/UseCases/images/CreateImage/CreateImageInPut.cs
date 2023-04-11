using MShop.Application.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MShop.Application.UseCases.images.CreateImage
{
    public class CreateImageInPut
    {
        public List<FileInputBase64>? Images {get; set; }

        public Guid ProductId { get; set; }
    }
}
