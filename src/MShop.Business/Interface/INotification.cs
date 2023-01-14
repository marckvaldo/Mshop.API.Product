using MShop.Business.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MShop.Business.Interface
{
    public interface INotification
    {
        bool HasErrors();

        List<MessageError> Errors();

       void AddNotifications(string error);


    }
}
