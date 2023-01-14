namespace MShop.Business.Validation
{
    public abstract class Notification
    {
        protected readonly Notifications _handler;

        protected Notification()
        {
            _handler = new Notifications();
        }
        public abstract Notifications Validate();
    }
}
