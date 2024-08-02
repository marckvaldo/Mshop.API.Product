using MShop.Application.UseCases.Category.Common;
using MShop.Application.UseCases.Category.CreateCategory;
using MShop.Core.Data;
using MShop.Core.DomainObject;
using MShop.Core.Message;
using MShop.Repository.Interface;

namespace MShop.Application.UseCases.Category.UpdateCategory
{
    public class UpdateCategory : Core.Base.BaseUseCase, IUpdateCategory
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
            

        public async Task<Result<CategoryModelOutPut>> Handle(UpdateCategoryInPut request, CancellationToken cancellationToken)
        {
            var category = await _categoryRepository.GetById(request.Id);
            if (NotifyErrorIfNull(category, "não foi possivel localizar a categoria da base de dados!"))
                return Result<CategoryModelOutPut>.Error();

            //NotifyExceptionIfNull(category, "não foi possivel localizar a categoria da base de dados!");


            category!.Update(request.Name);

            if (request.IsActive)
                category.Active();
            else
                category.Deactive();

            if(!category.IsValid(Notifications))
                return Result<CategoryModelOutPut>.Error();


            await _categoryRepository.Update(category,cancellationToken);
            await _unitOfWork.CommitAsync(cancellationToken);

            //return new CategoryModelOutPut(category.Id, category.Name, category.IsActive);
            //return CategoryModelOutPut.FromCategory(category);
            return Result<CategoryModelOutPut>.Success(CategoryModelOutPut.FromCategory(category));
        }
    }
}
