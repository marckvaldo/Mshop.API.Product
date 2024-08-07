﻿using MediatR;
using MShop.Application.UseCases.Category.Common;
using MShop.Core.DomainObject;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MShop.Application.UseCases.Category.UpdateCategory
{
    public class UpdateCategoryInPut: IRequest<Result<CategoryModelOutPut>>
    {
        public Guid Id { get; set; } 

        [Required(ErrorMessage = "O Campo {0} Obrigatório")]
        [StringLength(30, ErrorMessage = "O Campo {0} precisa ter no minimo {2} caracter e no maximo {1}", MinimumLength = 2)]
        public string Name { get; set; }

        public bool IsActive { get;  set; }
    }
}
