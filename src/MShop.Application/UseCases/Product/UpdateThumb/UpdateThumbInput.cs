using MediatR;
using MShop.Application.Common;
using MShop.Application.UseCases.Product.Common;
using MShop.Core.DomainObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MShop.Application.UseCases.Product.UpdateThumb
{
    public class UpdateThumbInPut : IRequest<Result<ProductModelOutPut>>
    {
        public Guid Id { get; set; }
        public FileInputBase64 Thumb { get; set; }   
        
    }
}
