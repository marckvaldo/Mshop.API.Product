using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MShop.Application.UseCases.Product.Common
{
    public class ProductModelOutPut
    {
        public ProductModelOutPut(Guid id, string description, string name, decimal price, string? imagem, decimal stock, bool isActive, Guid categoryId)
        {
            Description = description;
            Name = name;
            Price = price;
            Imagem = imagem;
            Stock = stock;
            IsActive = isActive;
            CategoryId = categoryId;
            Id = id;
        }
        public Guid Id { get; set; }

        [Required(ErrorMessage = "O Campo {0} Obrigatório")]
        [StringLength(100, ErrorMessage = "O Campo {0} precisa ter no minimo {2} caracter e no maximo {1}", MinimumLength = 2)]
        public string Description { get; set; }

        [Required(ErrorMessage = "O Campo {0} Obrigatório")]
        [StringLength(30, ErrorMessage = "O Campo {0} precisa ter no minimo {2} caracter e no maximo {1}", MinimumLength = 2)]
        public string Name { get; set; }

        public decimal Price { get; set; }

        public string? Imagem { get; set; }

        public decimal Stock { get;  set; }

        public bool IsActive { get; set; }

        public Guid CategoryId { get; set; }
    }
}
