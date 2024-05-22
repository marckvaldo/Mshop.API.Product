using MShop.Application.UseCases.Category.Common;
using MShop.Core.Message;
using MShop.Repository.Interface;

namespace MShop.Application.UseCases.Category.GetCategory
{
    public class GetCategory : Core.Base.BaseUseCase, IGetCategory
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
