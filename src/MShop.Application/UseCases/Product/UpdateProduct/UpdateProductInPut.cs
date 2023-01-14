using System.ComponentModel.DataAnnotations;

namespace MShop.Application.UseCases.Product.UpdateProducts
{
    public class UpdateProductInPut
    {
        public Guid Id { get; private set; }
    
        [Required(ErrorMessage = "O Campo {0} Obrigatório")]
        [StringLength(100, ErrorMessage = "O Campo {0} precisa ter no minimo {2} caracter e no maximo {1}", MinimumLength = 2)]
        public string Description { get; private set; }

        [Required(ErrorMessage = "O Campo {0} Obrigatório")]
        [StringLength(30, ErrorMessage = "O Campo {0} precisa ter no minimo {2} caracter e no maximo {1}", MinimumLength = 2)]
        public string Name { get; private set; }

        public decimal Price { get; private set; }

        public string? Imagem { get; private set; }

        public decimal Stock { get; private set; }

        public bool IsActive { get; private set; }

        public Guid CategoryId { get; private set; }
    }
}
