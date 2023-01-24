using MShop.Business.Validation;

namespace MShop.Business.Exceptions
{
    public class ApplicationValidationException : Exception
    {
       
        public ApplicationValidationException(
            string message
            ):base(message) 
        {
        }  
    }


}