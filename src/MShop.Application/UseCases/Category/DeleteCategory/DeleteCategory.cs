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

namespace MShop.Application.UseCases.Category.DeleteCategory
{
    public class DeleteCategory : BaseUseCase, IDeleteCategory
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IProductRepository _productRepository;
        public DeleteCategory(ICategoryRepository categoryRepository, IProductRepository productRepository,INotification notification) : base(notification)
        {
            _categoryRepository = categoryRepository;
            _productRepository= productRepository;
        }

        public async Task<CategoryModelOutPut> Handler(Guid id)
        {
            var category = await _categoryRepository.GetById(id);
            NotifyExceptionIfNull(category, "não foi possivel localizar a categoria da base de dados!");
            
            var products = await _productRepository.GetProductsByCategoryId(id);
            if (products?.Count > 0)
                NotifyException("Não é possivel excluir um categoria quando a mesma ja está relacionada com produtos");
           
            await _categoryRepository.DeleteById(category!);
            return new CategoryModelOutPut(category!.Id, category.Name, category.IsActive);
        }
    }
}
