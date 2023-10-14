using MediatR;
using MShop.Application.UseCases.Product.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MShop.Application.UseCases.Product.UpdateStockProduct
{
    public class UpdateStockProductInPut : IRequest<ProductModelOutPut>
    {
        [Required(ErrorMessage = "O Campo {0} Obrigatório")]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "O Campo {0} Obrigatório")]
        public decimal Stock { get; set; }
    }
}
