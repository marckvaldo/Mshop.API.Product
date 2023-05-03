using MShop.Application.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MShop.Application.UseCases.Product.UpdateThumb
{
    public class UpdateThumbInput
    {
        public Guid Id { get; set; }
        public FileInputBase64 Thumb { get; set; }   
        
    }
}
