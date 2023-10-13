using MShop.Application.UseCases.Category.Common;
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
        private readonly IUnitOfWork _unitOfWork;
        public UpdateCategory(
            ICategoryRepository categoryRepository, 
            INotification notification,
            IUnitOfWork unitOfWork
            ) : base(notification)
        {
            _categoryRepository = categoryRepository;
            _unitOfWork = unitOfWork;
        }
            

        public async Task<CategoryModelOutPut> Handle(UpdateCategoryInPut request, CancellationToken cancellationToken)
        {
            var category = await _categoryRepository.GetById(request.Id);
            NotifyExceptionIfNull(category, "não foi possivel localizar a categoria da base de dados!");

            category!.Update(request.Name);

            if (request.IsActive)
                category.Active();
            else
                category.Deactive();

            category.IsValid(Notifications);
            await _categoryRepository.Update(category,cancellationToken);
            await _unitOfWork.CommitAsync(cancellationToken);

            //return new CategoryModelOutPut(category.Id, category.Name, category.IsActive);
            return CategoryModelOutPut.FromCategory(category);
        }
    }
}
