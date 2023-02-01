using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using MShop.Application.UseCases.Category.GetCatetory;
using MShop.Application.UseCases.Product.Common;
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
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<GetCategoryWithProductsOutPut> Handler(Guid id)
        {
            var category = await  _categoryRepository.GetCategoryProducts(id);
            if(category == null)
            {
                Notify("não foi possivel localizar a categoria da base de dados!");
                throw new ApplicationValidationException("");
            }
            category.IsValid(_notifications);

            List<ProductModelOutPut> listProdutos = new List<ProductModelOutPut>();

            foreach(var item in category.Products)
            {
                listProdutos.Add(new ProductModelOutPut(
                    item.Id,
                    item.Description,
                    item.Name,
                    item.Price,
                    item.Imagem,
                    item.Stock,
                    item.IsActive,
                    item.CategoryId
                    ));
            }

            return new GetCategoryWithProductsOutPut(
                id, 
                category.Name, 
                category.IsActive,
                listProdutos);
        }
    }
}
