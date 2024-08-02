using MediatR;
using MShop.Application.UseCases.Category.Common;
using MShop.Core.Data;
using MShop.Core.DomainObject;
using Domain = MShop.Core.Message;
using MShop.Repository.Interface;
using MShop.Repository.Repository;

namespace MShop.Application.UseCases.Category.CreateCategory
{
    public class CreateCategory : Core.Base.BaseUseCase, ICreateCategory
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateCategory(
            Domain.INotification notification,
            ICategoryRepository categoryRepository,
            IUnitOfWork unitOfWork) : base(notification)
        {
            _categoryRepository = categoryRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<CategoryModelOutPut>> Handle(CreateCategoryInPut request, CancellationToken cancellationToken)
        {
            var category = new Business.Entity.Category(request.Name, request.IsActive);

            if(!category.IsValid(Notifications))
                return Result<CategoryModelOutPut>.Error(Notifications);


            var isThereCatetory = await _categoryRepository.GetByName(request.Name);
            if (isThereCatetory is not null)
            {
                Notify("Categoria ja existe na base de dados");
                return Result<CategoryModelOutPut>.Error(Notifications);
            }
                
       

            await _categoryRepository.Create(category,cancellationToken);
            await _unitOfWork.CommitAsync(cancellationToken);
           
            //return new CategoryModelOutPut(category.Id, category.Name, category.IsActive);
            //return CategoryModelOutPut.FromCategory(category);
            return Result<CategoryModelOutPut>.Success(CategoryModelOutPut.FromCategory(category));

        }

    }
}
