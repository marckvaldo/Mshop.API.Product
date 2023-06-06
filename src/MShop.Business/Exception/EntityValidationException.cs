using MShop.Business.Interface;

namespace MShop.Business.Exceptions
{
    public class EntityValidationException : ApplicationException
    {
        private readonly INotification _notification;
        public EntityValidationException(
            string message
            ) : base(message)
        {
        }
    }


}