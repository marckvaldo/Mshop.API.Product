using MShop.Business.Exceptions;
using MShop.Business.Interface;
using MShop.Business.Validator;

namespace MShop.Business.Entity
{
    public class Image : Entity
    {
        public Image(string fileName, Guid productId)
        {
            FileName = fileName;
            ProductId = productId;
        }

        public string FileName { get; set; }
        public Guid ProductId { get; set; }

        public void IsValid(INotification notification)
        {
            var imageValidate = new ImageValidador(this,notification);
            imageValidate.Validate();
            if(notification.HasErrors())
            {
                throw new EntityValidationException("Validation errors");
            }
        }

        public void UpdateUrlImage(string fileName)
        {
            FileName = fileName;
        }

    }
}
