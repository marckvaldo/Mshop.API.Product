using Business = MShop.Business.Entity;
using MShop.Business.Interface;
using MShop.Business.Interface.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MShop.Application.UseCases.Category.Common;

namespace MShop.Application.UseCases.Category.CreateCategory
{
    public class CreateCategory : BaseUseCase, ICreateCategory
    {
        private readonly ICategoryRepository _categoryRepository;
        public CreateCategory(INotification notification, ICategoryRepository categoryRepository) : base(notification)
           => _categoryRepository = categoryRepository;

        public async Task<CategoryModelOutPut> Handler(CreateCategoryInPut request)
        {
            var category = new Business.Entity.Category(request.Name, request.IsActive);

            category.IsValid(Notifications);
            await _categoryRepository.Create(category);
           
            return new CategoryModelOutPut(category.Id, category.Name, category.IsActive);

        }
    }
}
