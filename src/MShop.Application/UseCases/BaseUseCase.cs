using MShop.Business.Exceptions;
using MShop.Business.Interface;
using MShop.Business.Validation;

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

        protected void NotifyStop(string menssagem)
        {
            Notifications.AddNotifications(menssagem);
            throw new ApplicationValidationException("Erro Applications");
        }
    }
}
