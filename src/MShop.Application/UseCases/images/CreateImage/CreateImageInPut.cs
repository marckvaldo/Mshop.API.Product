using MediatR;
using MShop.Application.Common;
using MShop.Application.UseCases.Images.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MShop.Application.UseCases.Images.CreateImage
{
    public class CreateImageInPut : IRequest<ListImageOutPut>
    {
        public List<FileInputBase64>? Images {get; set; }

        public Guid ProductId { get; set; }
    }
}
