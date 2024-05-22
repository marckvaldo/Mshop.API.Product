using MShop.Business.Entity;
using MShop.Core.DomainObject;
using MShop.Core.Message;

namespace MShop.Business.Validator
{
    public class CategoryValidador : Notification
    {
        private readonly Category _category;
        public CategoryValidador(Category category, INotification notifications) : base(notifications)
        {
            _category = category;
        }

        public override INotification Validate()
        {
            ValidationDefault.NotNullOrEmpty(_category.Name, nameof(_category.Name), _notifications);
            ValidationDefault.MaxLength(_category.Name, 30, nameof(_category.Name), _notifications);
            ValidationDefault.MinLength(_category.Name, 3, nameof(_category.Name), _notifications);

            return _notifications;
        }
    }
}
