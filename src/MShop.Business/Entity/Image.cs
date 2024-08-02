﻿using MShop.Business.Validator;
using CoreException = MShop.Core.Exception;
using Core =MShop.Core.Message;
using MShop.Core.Message;

namespace MShop.Business.Entity
{
    public class Image : Core.DomainObject.Entity
    {
        public Image(string fileName, Guid productId)
        {
            FileName = fileName;
            ProductId = productId;
        }

        public string FileName { get; set; }
        public Guid ProductId { get; set; }

        /*public override void IsValid(Core.Message.INotification notification)
        {
            var imageValidate = new ImageValidador(this, notification);
            imageValidate.Validate();
            if (notification.HasErrors())
            {
                throw new CoreException.EntityValidationException("Validation errors");
            }
        }*/

        public override bool IsValid(INotification notification)
        {
            var productValidador = new ImageValidador(this, notification);
            productValidador.Validate();
            return !notification.HasErrors();

        }

        public void UpdateUrlImage(string fileName)
        {
            FileName = fileName;
        }

    }
}
