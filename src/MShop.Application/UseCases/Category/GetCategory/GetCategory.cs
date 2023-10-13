using MShop.Application.UseCases.Category.Common;
using MShop.Application.UseCases.Category.GetCategory;
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

namespace MShop.Application.UseCases.Category.GetCategory
{
    public class GetCategory : BaseUseCase, IGetCategory
    {
        private readonly ICategoryRepository _categoryRepository;
        public GetCategory(INotification notification, ICategoryRepository categoryRepository) : base(notification)
            => _categoryRepository = categoryRepository;

        public async Task<CategoryModelOutPut> Handle(GetCategoryInPut request, CancellationToken cancellationToken)
        {
            var category = await  _categoryRepository.GetById(request.Id);            
            NotifyExceptionIfNull(category, "não foi possivel localizar a categoria da base de dados!");

            category!.IsValid(Notifications);
            return CategoryModelOutPut.FromCategory(category);  
            //return new CategoryModelOutPut(request.Id, category.Name, category.IsActive);
        }

    }
}
