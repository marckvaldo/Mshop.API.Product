using MShop.Business.Entity;
using MShop.Business.Exception;
using MShop.Business.Exceptions;
using MShop.Business.Interface;
using MShop.Business.Validation;
using System;

namespace MShop.Application.UseCases
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
