using MShop.Business.Exceptions;
using MShop.Business.Interface;

namespace MShop.Business.Validation
{

    public static class ValidationDefault
    {
        public static void NotNull(string target, string fieldName, INotification notification)
        {
            if (target is null)
                notification.AddNotifications($"O {fieldName} não deve ser null");
        }

        public static void NotNullOrEmpty(string target, string fieldName, INotification notification)
        {
            if (String.IsNullOrWhiteSpace(target))
                notification.AddNotifications($"O {fieldName} não pode set vario ou null");
        }

        public static void MinLength(string target, int minLength, string fieldName, INotification notification)
        {
            NotNull(target, fieldName, notification);

            if (target.Length < minLength)
                notification.AddNotifications($"O {fieldName} não pode ser menor que {minLength} characters");
        }

        public static void MaxLength(string target, int maxLength, string fieldName, INotification notification)
        {
            NotNull(target, fieldName, notification);

            if (target.Length > maxLength)
                notification.AddNotifications($"O {fieldName} não pode ser maior que {maxLength} characters");
        }

        //Numbers
        public static void IsPositive(decimal target, string fieldName, INotification notification)
        {
            if (target < 0)
                notification.AddNotifications($"O {fieldName} não é um numero positivo");
        }

        public static void IsBiggerThan(decimal target, decimal value, string fieldName, INotification notification)
        {
            if (target < value)
                notification.AddNotifications($"O {fieldName} não pode ser menor que {value}");
        }

        public static void IsBiggerOrEqualThan(decimal target, decimal value, string fieldName, INotification notification)
        {
            if (target <= value)
                notification.AddNotifications($"O {fieldName} não pode ser menor ou igual {value}");
        }

        public static void IsLessThan(decimal target, decimal value, string fieldName, INotification notification)
        {
            if (target > value)
                notification.AddNotifications($"O {fieldName} não pode ser maior que {value}");
        }

        public static void IsLessOrEqualThan(decimal target, decimal value, string fieldName, INotification notification)
        {
            if (target >= value)
                notification.AddNotifications($"O {fieldName} não pode ser maior o igual a {value}");
        }

        public static void NotEqual(decimal target, decimal value, string fieldName, INotification notification, string? messagem=null)
        {
            if (messagem is null)
                messagem = $"O {fieldName} não é igual a {value}";

            if (target != value)
                notification.AddNotifications(messagem);
        }

    }
}
