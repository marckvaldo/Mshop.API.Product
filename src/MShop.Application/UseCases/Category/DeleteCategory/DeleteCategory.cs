using MShop.Application.UseCases.Category.Common;
using MShop.Core.Base;
using MShop.Core.Data;
using MShop.Core.Message;
using MShop.Repository.Interface;

namespace MShop.Application.UseCases.Category.DeleteCategory
{
    public class DeleteCategory : BaseUseCase, IDeleteCategory
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IProductRepository _productRepository;
        private readonly IUnitOfWork _unitOfWork;
        public DeleteCategory(
            ICategoryRepository categoryRepository, 
            IProductRepository productRepository,
            INotification notification, 
            IUnitOfWork unitOfWork) : base(notification)
        {
            _categoryRepository = categoryRepository;
            _productRepository = productRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<CategoryModelOutPut> Handle(DeleteCategoryInPut request, CancellationToken cancellationToken)
        {
            var category = await _categoryRepository.GetById(request.Id);
            NotifyExceptionIfNull(category, "não foi possivel localizar a categoria da base de dados!");
            
            var products = await _productRepository.GetProductsByCategoryId(request.Id);
            if (products?.Count > 0)
                NotifyException("Não é possivel excluir um categoria quando a mesma ja está relacionada com produtos");
           
            await _categoryRepository.DeleteById(category!,cancellationToken);
            await _unitOfWork.CommitAsync(cancellationToken);

            //return new CategoryModelOutPut(category!.Id, category.Name, category.IsActive);
            return CategoryModelOutPut.FromCategory(category!);
        }


    }
}
