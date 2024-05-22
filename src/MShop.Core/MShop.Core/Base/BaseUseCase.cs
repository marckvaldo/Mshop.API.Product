using MShop.Core.Exception;
using MShop.Core.Message;


namespace MShop.Core.Base
{
    public abstract class BaseUseCase: Notifications
    {
        protected readonly INotification Notifications;

        protected BaseUseCase(INotification notification)
        {
            Notifications = notification;
        }

        protected void Notify(string menssagem)
        {
            Notifications.AddNotifications(menssagem);
        }

        protected void NotifyException(string menssagem)
        {
            Notifications.AddNotifications(menssagem);
            throw new ApplicationValidationException("Error");
        }

        protected void NotifyExceptionIfNull(object? @object,  string menssagem)
        {
            if (@object == null)
            {
                Notifications.AddNotifications(menssagem);
                throw new ApplicationValidationException("Error");
            }
                
        }

        
    }
}
