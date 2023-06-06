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
