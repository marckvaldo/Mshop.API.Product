using MShop.Application.Common;
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
            if(category == null)
            {
                Notify("Não foi possivel localizar a categoria na base de dados");
                throw new ApplicationValidationException("");
            }

            category.Update(request.Name);

            if (request.IsActive)
                category.Active();
            else
                category.Deactive();

            await _categoryRepository.Update(category);

            return new CategoryModelOutPut(category.Id, category.Name, category.IsActive);
                
        }
    }
}
