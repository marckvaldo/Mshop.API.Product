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
            _notification.HandleError(menssagem);
        }

        /*protected void Notificar(List<NotificationError> resultValidationError)
        {
            foreach(var error in resultValidationError)
            {
                Notificar(error.Message);
            }
        }*/

        protected bool Validate(Validation.Validator validation)
        {
            var result = validation.Validate();
            if (result.HasErros()) return true;

            //Notificar(result.Erros());

            return false;
        }
    }
}
