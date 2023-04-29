﻿using MShop.Application.UseCases.Category.Common;
using MShop.Business.Entity;
using MShop.Business.Exception;
using MShop.Business.Exceptions;
using MShop.Business.Interface;
using MShop.Business.Interface.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MShop.Application.UseCases.Category.UpdateCategory
{
    public class UpdateCategory : BaseUseCase, IUpdateCategory
    {
        private readonly ICategoryRepository _categoryRepository;
        public UpdateCategory(ICategoryRepository categoryRepository, INotification notification) : base(notification)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<CategoryModelOutPut> Handler(UpdateCategoryInPut request)
        {
            var category = await _categoryRepository.GetById(request.Id);

            Notify("não foi possivel localizar a categoria da base de dados!");
            NotFoundException.ThrowIfnull(category, "your search returned null");

            /*if(category == null)
            {
                Notify("Não foi possivel localizar a categoria na base de dados");
                throw new ApplicationValidationException("");
            }*/

            category.Update(request.Name);

            if (request.IsActive)
                category.Active();
            else
                category.Deactive();

            category.IsValid(Notifications);

            await _categoryRepository.Update(category);

            return new CategoryModelOutPut(category.Id, category.Name, category.IsActive);
                
        }
    }
}
