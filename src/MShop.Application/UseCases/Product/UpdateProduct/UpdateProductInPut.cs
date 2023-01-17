using System.ComponentModel.DataAnnotations;

namespace MShop.Application.UseCases.Product.UpdateProducts
{
    public class UpdateProductInPut
    {
        [Required(ErrorMessage = "O Campo {0} Obrigatório")]
        public Guid Id { get; set; }
    
        [Required(ErrorMessage = "O Campo {0} Obrigatório")]
        [StringLength(1000, ErrorMessage = "O Campo {0} precisa ter no minimo {2} caracter e no maximo {1}", MinimumLength = 2)]
        public string Description { get; set; }

        [Required(ErrorMessage = "O Campo {0} Obrigatório")]
        [StringLength(100, ErrorMessage = "O Campo {0} precisa ter no minimo {2} caracter e no maximo {1}", MinimumLength = 2)]
        public string Name { get; set; }

        public decimal Price { get; set; }

        public string? Imagem { get; set; }

        public bool IsActive { get; set; }

        public Guid CategoryId { get; set; }
    }
}
