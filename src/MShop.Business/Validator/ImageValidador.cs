using MShop.Business.Entity;
using MShop.Business.Interface;
using MShop.Business.Validation;
using MShop.Business.ValueObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MShop.Business.Validator
{
    public class ImageValidador : Notification
    {
        private readonly Entity.Image _images;
        public ImageValidador(Entity.Image images, INotification notification) : base(notification)
        {
            _images = images;
        }

        public override INotification Validate()
        {
            ValidationDefault.NotNullOrEmpty(_images.FileName, nameof(_images.FileName), _notifications);
            ValidationDefault.NotNullGuid(_images.ProductId, nameof(_images.ProductId), _notifications);
            return _notifications;
        }
    }
}
