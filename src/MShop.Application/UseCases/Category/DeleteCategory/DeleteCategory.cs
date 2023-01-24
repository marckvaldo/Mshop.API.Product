using MShop.Application.Common;
using MShop.Business.Exceptions;
using MShop.Business.Interface;
using MShop.Business.Interface.Repository;
using MShop.Repository.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MShop.Application.UseCases.Category.DeleteCategory
{
    public class DeleteCategory : BaseUseCase, IDeleteCategory
    {
        private readonly ICategoryRepository _categoryRepository;
        public DeleteCategory(ICategoryRepository categoryRepository, INotification notification) : base(notification)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<CategoryModelOutPut> Handler(Guid id)
        {
            var category = await _categoryRepository.GetById(id);
            if(category == null)
            {
                Notify("Não foi possivel localizar a categoria na base de dados");
                throw new ApplicationValidationException("");
            }

            await _categoryRepository.DeleteById(category);

            return new CategoryModelOutPut(category.Id, category.Name, category.IsActive);
        }
    }
}
