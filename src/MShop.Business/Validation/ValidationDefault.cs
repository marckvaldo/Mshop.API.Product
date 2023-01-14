using MShop.Business.Exceptions;

namespace MShop.Business.Validation
{

    public static class ValidationDefault
    {
        public static void NotNull(string target, string fieldName)
        {
            if (target is null)
                throw new EntityValidationException($"O {fieldName} não deve ser null");
        }

        public static void NotNullOrEmpty(string target, string fieldName)
        {
            if (String.IsNullOrWhiteSpace(target))
                throw new EntityValidationException($"O {fieldName} não pode set vario ou null");
        }

        public static void MinLength(string target, int minLength, string fieldName)
        {
            NotNull(target, fieldName); 

            if (target.Length < minLength)
                throw new EntityValidationException($"O {fieldName} não pode ser menor que {minLength} characters");
        }

        public static void MaxLength(string target, int maxLength, string fieldName)
        {
            NotNull(target, fieldName);

            if (target.Length > maxLength)
                throw new EntityValidationException($"O {fieldName} não pode ser maior que {maxLength} characters");
        }

        //Numbers
        public static void IsPositiveNumber(decimal target, string fieldName)
        {
            if (target < 0)
                throw new EntityValidationException($"O {fieldName} não é positivo");
        }

        public static void IsGreaterThan(decimal target, decimal value, string fieldName)
        {
            if (target < value)
                throw new EntityValidationException($"O {fieldName} não pode ser menor que {value}");
        }

        public static void IsLessThan(decimal target, decimal value, string fieldName)
        {
            if (target > value)
                throw new EntityValidationException($"O {fieldName} não pode ser maior que {value}");
        }

        public static void NotEqual(decimal target, decimal value, string fieldName, string? messagem=null)
        {
            if (messagem is null)
                messagem = $"O {fieldName} não é igual a {value}";

            if (target != value)
                throw new EntityValidationException(messagem);
        }

    }
}
