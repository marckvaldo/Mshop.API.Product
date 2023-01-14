using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MShop.Business.Validation
{
    public class MessageError
    {
        public string Message { get; }
        public MessageError(string message) 
        {
            Message = message;
        }    
    }
}
