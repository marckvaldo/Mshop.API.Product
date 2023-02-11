using MShop.Application.UseCases.Product.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MShop.EndToEndTest.API.Product.Common
{
    public class CustomResponseErro
    {

        public List<string> Errors { get; set; }

        public bool Success { get; set; }

        public CustomResponseErro(List<string> errors, bool success)
        {
            Errors = errors;
            Success = success;
        }

        
    }
}
