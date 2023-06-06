namespace MShop.Business.Exceptions
{
    public class ApplicationValidationException : ApplicationException
    {

        public ApplicationValidationException(
            string message
            ) : base(message)
        {
        }
    }


}