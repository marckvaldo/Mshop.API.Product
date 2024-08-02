using MShop.Core.Exception;
using MShop.Core.Message;
using System;


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

        protected bool NotifyErrorIfNull(object? @object, string menssagem)
        {
            if (@object == null)
            {
                Notifications.AddNotifications(menssagem);
                return true;
            }
            return false;
        }
    }
}
