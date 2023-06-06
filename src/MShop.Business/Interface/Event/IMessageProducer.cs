namespace MShop.Business.Interface.Event
{
    public interface IMessageProducer
    {
        Task SendMessageAsync<T>(T message);
    }
}
