﻿using MediatR;
using MShop.Application.UseCases.Category.Common;
using MShop.Application.UseCases.Category.GetCategory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MShop.Application.UseCases.Category.GetCategory
{
    public interface IGetCategory : IRequestHandler<GetCategoryInPut, CategoryModelOutPut>
    {
        Task<CategoryModelOutPut> Handle(GetCategoryInPut id, CancellationToken cancellationToken);
    }
}
