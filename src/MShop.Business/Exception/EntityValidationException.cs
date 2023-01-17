using MShop.Business.Validation;

namespace MShop.Business.Exceptions
{
    public class EntityValidationException: Exception
    {
        public IReadOnlyCollection<MessageError>? Errors { get; }
        public EntityValidationException(string message, IReadOnlyCollection<MessageError>? error = null):base(message) 
        {
            Errors = error;
        }  
    }


}