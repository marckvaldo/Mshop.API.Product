using MShop.Application.UseCases.Category.Common;
using MShop.Business.Exception;
using MShop.Business.Exceptions;
using MShop.Business.Interface;
using MShop.Business.Interface.Repository;
using MShop.Repository.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MShop.Application.UseCases.Category.GetCatetory
{
    public class GetCategory : BaseUseCase, IGetCategory
    {
        private readonly ICategoryRepository _categoryRepository;
        public GetCategory(INotification notification, ICategoryRepository categoryRepository) : base(notification)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<CategoryModelOutPut> Handler(Guid id)
        {
            var category = await  _categoryRepository.GetById(id);
            
            NotFoundException.ThrowIfnull(category, "não foi possivel localizar a categoria da base de dados!");

            category.IsValid(Notifications);
            return new CategoryModelOutPut(id, category.Name, category.IsActive);
        }
    }
}
