using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using MShop.Application.UseCases.Category.GetCategory;
using MShop.Application.UseCases.Category.GetCatetoryWithProducts;
using MShop.Application.UseCases.Product.Common;
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

namespace MShop.Application.UseCases.GetCatetoryWithProducts.GetCatetory
{
    public class GetCategoryWithProducts : BaseUseCase, IGetCategoryWithProducts
    {
        private readonly ICategoryRepository _categoryRepository;
        public GetCategoryWithProducts(INotification notification, ICategoryRepository categoryRepository) : base(notification)
          => _categoryRepository = categoryRepository;
        public async Task<GetCategoryWithProductsOutPut> Handle(GetCategoryWithProductsInPut request, CancellationToken cancellationToken)
        {
            var category = await  _categoryRepository.GetCategoryProducts(request.Id);
            NotifyExceptionIfNull(category, "não foi possivel localizar a categoria da base de dados!");
            category.IsValid(Notifications);

            List<ProductModelOutPut> listProdutos = new List<ProductModelOutPut>();
            foreach(var item in category.Products)
            {
                listProdutos.Add(new ProductModelOutPut(
                    item.Id,
                    item.Description,
                    item.Name,
                    item.Price,
                    item.Thumb?.Path,
                    item.Stock,
                    item.IsActive,
                    item.CategoryId
                    ));
            }

            /*return new GetCategoryWithProductsOutPut(
                request.Id, 
                category.Name, 
                category.IsActive,
                listProdutos);*/

            return GetCategoryWithProductsOutPut.FromCategory(category, listProdutos);
        }
    }
}
