using MShop.Business.Interface;
using MShop.Business.Validation;


namespace MShop.Business.Service
{
    public abstract class BaseService
    {
        private readonly INotification _notification;

        protected BaseService(INotification notification)
        {
            _notification = notification;
        }

        protected void Notificar(string menssagem)
        {
            _notification.AddNotifications(menssagem);
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
