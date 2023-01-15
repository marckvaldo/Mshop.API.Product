using MShop.Business.Interface;

namespace MShop.Business.Validation
{
    public abstract class Notification
    {
        protected readonly INotification _notifications;

        protected Notification(INotification notifications)
        {
            _notifications = notifications;
        }
        public abstract INotification Validate();
    }
}
