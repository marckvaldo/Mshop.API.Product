using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using MShop.Core.Message;
using MShop.ProductAPI.Extension;

namespace MShop.ProductAPI.Controllers
{
    [ApiController]
    public abstract class MainController : ControllerBase
    {
        private readonly INotification _notification;

        protected MainController(INotification notification)
        {
            _notification= notification;
        }

        protected bool OperationIsValid()
        {
            return !_notification.HasErrors();
        }
        
        protected ActionResult CustomResponse(ModelStateDictionary modelState)
        {
            if (!modelState.IsValid) NotifyInvalidModelError(modelState);

            return CustomResponse();
        }

        protected ActionResult CustomResponse(object result = null)
        {
            if(OperationIsValid())
            {
                return Ok(ExtensionResponse.Success(result));
            }

            return BadRequest(ExtensionResponse.Error(_notification.Errors().Select(x => x.Message).ToList()));
        }

        protected void NotifyInvalidModelError(ModelStateDictionary modelState)
        {
            var errors = modelState.Values.SelectMany(e => e.Errors);
            
            foreach(var error in errors)
            {
                var errorMsg = error.Exception == null ? error.ErrorMessage : error.Exception.Message;
                Notify(errorMsg);
            }
        }

        protected void Notify(string messagem)
        {
            if(messagem.Length > 0)
                _notification.AddNotifications(messagem);
        }
    }
}
