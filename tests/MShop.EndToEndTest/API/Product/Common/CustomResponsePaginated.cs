using MShop.Application.UseCases.Product.Common;
using MShop.Business.Interface.Paginated;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MShop.EndToEndTest.API.Product.Common
{
    public class CustomResponsePaginated<TResult>
    {

        public Paginated<TResult> Data { get; set; }

        public bool Success { get; set; }

        public CustomResponsePaginated(Paginated<TResult> data, bool success)
        {
            Data = data;
            Success = success;
        }

        
    }
}
