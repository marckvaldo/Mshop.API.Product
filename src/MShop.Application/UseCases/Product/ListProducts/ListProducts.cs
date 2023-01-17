using MShop.Application.Common;
using MShop.Business.Interface;
using MShop.Business.Interface.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MShop.Application.UseCases.Product.ListProducts
{
    public class ListProducts : BaseUseCase, IListProducts
    {
        private readonly IProductRepository _productRepostory;

        public ListProducts(IProductRepository productRepostory, INotification notification) : base(notification)
        {
            _productRepostory = productRepostory;   
        }

        public async Task<List<ProductModelOutPut>> Handle()
        {
            var products = await _productRepostory.GetValuesList();

            List<ProductModelOutPut>  ListProducts = new List<ProductModelOutPut>();
            foreach(var item in products)
            {
               ListProducts.Add(new ProductModelOutPut( 
                   item.Id, 
                   item.Description, 
                   item.Name, 
                   item.Price, 
                   item.Imagem, 
                   item.Stock, 
                   item.IsActive, 
                   item.CategoryId));
            }

            return ListProducts;
        }
    }
}
