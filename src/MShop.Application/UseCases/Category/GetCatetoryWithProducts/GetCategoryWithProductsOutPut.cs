using MShop.Application.Common;
using MShop.Application.UseCases.Category.GetCatetory;
using MShop.Business.Exceptions;
using MShop.Business.Interface;
using MShop.Business.Interface.Repository;
using MShop.Repository.Repository;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MShop.Application.UseCases.GetCatetoryWithProducts.GetCatetory
{
    public class GetCategoryWithProductsOutPut
    {
        public GetCategoryWithProductsOutPut(Guid id, string name, bool isActive, List<ProductModelOutPut> products)
        {
            Id = id;
            Name = name;
            IsActive = isActive;
            Products = products;
        }

        public Guid Id { get; private set; }

        [Required(ErrorMessage = "O Campo {0} Obrigatório")]
        [StringLength(30, ErrorMessage = "O Campo {0} precisa ter no minimo {2} caracter e no maximo {1}", MinimumLength = 2)]
        public string Name { get; private set; }

        public bool IsActive { get; private set; }

        public List<ProductModelOutPut> Products { get; private set; }


    }
}
