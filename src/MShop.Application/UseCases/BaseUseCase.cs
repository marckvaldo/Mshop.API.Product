using MShop.Business.Interface;
using MShop.Business.Validation;

namespace MShop.Application.UseCases
{
    public abstract class BaseUseCase: Notifications
    {
        private readonly INotification _notifications;

        protected BaseUseCase(INotification notification)
        {
            _notifications = notification;
        }

        protected void Notify(string menssagem)
        {
            _notifications.AddNotifications(menssagem);
        }

        protected bool IsValid(Notification validation)
        {
            var result = validation.Validate();
            if (result.HasErrors()) return false;

            //Notificar(result.Erros());

            return true;
        }

   
    }
}
