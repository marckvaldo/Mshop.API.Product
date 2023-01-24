using MShop.Application.Common;
using Business = MShop.Business.Entity;
using MShop.Business.Interface;
using MShop.Business.Interface.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MShop.Application.UseCases.Category.CreateCategory
{
    public class CreateCategory : BaseUseCase, ICreateCategory
    {
        private readonly ICategoryRepository _categoryRepository;
        public CreateCategory(INotification notification, ICategoryRepository categoryRepository) : base(notification)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<CategoryModelOutPut> Handler(CreateCategoryInPut request)
        {
            var category = new Business.Entity.Category(request.Name, request.IsActive);

            category.IsValid(_notifications);

            await _categoryRepository.Create(category);
            var newCategory = await _categoryRepository.GetLastRegister(x => x.Name == category.Name);

            return new CategoryModelOutPut(newCategory.Id, newCategory.Name, newCategory.IsActive);

        }
    }
}
