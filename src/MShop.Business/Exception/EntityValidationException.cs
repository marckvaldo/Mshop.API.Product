using MShop.Business.Validation;

namespace MShop.Business.Exceptions
{
    public class EntityValidationException: ApplicationException
    {
       
        public EntityValidationException(
            string message
            ):base(message) 
        {
        }  
    }


}