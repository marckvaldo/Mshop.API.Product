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

        /*protected bool IsValid(Notification validation)
        {
            var result = validation.Validate();
            if (result.HasErrors()) return false;

            //Notificar(result.Erros());

            return true;
        }*/

   
    }
}
