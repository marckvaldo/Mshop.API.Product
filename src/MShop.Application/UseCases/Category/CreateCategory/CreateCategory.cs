using MShop.Application.UseCases.Category.Common;
using MShop.Core.Data;
using MShop.Core.Message;
using MShop.Repository.Interface;

namespace MShop.Application.UseCases.Category.CreateCategory
{
    public class CreateCategory : Core.Base.BaseUseCase, ICreateCategory
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

        public async Task<CategoryModelOutPut> Handle(CreateCategoryInPut request, CancellationToken cancellationToken)
        {
            var category = new Business.Entity.Category(request.Name, request.IsActive);

            category.IsValid(Notifications);
            await _categoryRepository.Create(category,cancellationToken);
            await _unitOfWork.CommitAsync(cancellationToken);
           
            //return new CategoryModelOutPut(category.Id, category.Name, category.IsActive);
            return CategoryModelOutPut.FromCategory(category);

        }
    }
}
