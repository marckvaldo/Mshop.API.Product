﻿using MediatR;
using MShop.Application.UseCases.Category.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MShop.Application.UseCases.Category.DeleteCategory
{
    public interface IDeleteCategory : IRequestHandler<DeleteCategoryInPut, CategoryModelOutPut>
    {
        Task<CategoryModelOutPut> Handle(DeleteCategoryInPut request, CancellationToken cancellationToken);
    }
}
