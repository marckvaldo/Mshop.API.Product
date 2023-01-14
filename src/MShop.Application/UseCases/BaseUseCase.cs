using MShop.Business.Interface;
using MShop.Business.Validation;

namespace MShop.Application.UseCases
{
    public abstract class BaseUseCase
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
        protected bool Validate(Notification validation)
        {
            var result = validation.Validate();
            if (result.HasErrors()) return true;

            //Notificar(result.Erros());

            return false;
        }
    }
}
