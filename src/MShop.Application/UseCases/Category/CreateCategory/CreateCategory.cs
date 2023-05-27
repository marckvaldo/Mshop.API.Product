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
        private readonly IUnitOfWork _unitOfWork;

        public CreateCategory(
            INotification notification,
            ICategoryRepository categoryRepository,
            IUnitOfWork unitOfWork) : base(notification)
        {
            _categoryRepository = categoryRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<CategoryModelOutPut> Handler(CreateCategoryInPut request, CancellationToken cancellationToken)
        {
            var category = new Business.Entity.Category(request.Name, request.IsActive);

            category.IsValid(Notifications);
            await _categoryRepository.Create(category,cancellationToken);
            await _unitOfWork.CommitAsync(cancellationToken);
           
            return new CategoryModelOutPut(category.Id, category.Name, category.IsActive);

        }
    }
}
