using MShop.Business.Validation;

namespace MShop.Business.Interface
{
    public interface INotification
    {
        bool HasErrors();

        List<MessageError> Errors();

        void AddNotifications(string error);


    }
}
