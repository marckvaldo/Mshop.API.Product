using MShop.Business.Interface;
using MShop.Business.Validation;

namespace MShop.Business.Exceptions
{
    public class EntityValidationException: ApplicationException
    {
       private readonly INotification _notification;
        public EntityValidationException(
            string message
            ):base(message) 
        {
        }  
    }


}