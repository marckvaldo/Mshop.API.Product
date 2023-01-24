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
            if(category == null)
            {
                Notify("não foi possivel localizar a categoria da base de dados!");
                throw new ApplicationValidationException("");
            }
            category.IsValid(_notifications);
            return new CategoryModelOutPut(id, category.Name, category.IsActive);
        }
    }
}
