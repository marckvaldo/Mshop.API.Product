using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MShop.EndToEndTest.API.Product.Common
{
    public class CustomResponse<TResult>
    {

        public TResult Result { get; set; }

        public string Data { get; set; }

        public string Success { get; set; }

        public CustomResponse(TResult result, string data, string success)
        {
            Result = result;
            Data = data;
            Success = success;
        }

        
    }
}
