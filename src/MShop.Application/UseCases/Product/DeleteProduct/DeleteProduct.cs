using MShop.Application.Common;
using MShop.Business.Exceptions;
using MShop.Business.Interface;
using MShop.Business.Interface.Repository;


namespace MShop.Application.UseCases.Product.DeleteProduct
{
    public class DeleteProduct : BaseUseCase, IDeleteProduct
    {
        private readonly IProductRepository _productRespository;
 
        public DeleteProduct(IProductRepository productRespository, INotification notification):base(notification)
        {
            _productRespository = productRespository;
        }

        public async Task<ProductModelOutPut> Handle(Guid request)
        {
            var product = await _productRespository.GetById(request);
            if(product == null)
            {
                Notify("Não foi possivel localizar o produto no base de dados");
                throw new EntityValidationException("There are erros", Errors());
            }

            await _productRespository.DeleteById(product);
            return new ProductModelOutPut(
                product.Id, 
                product.Description,
                product.Name, 
                product.Price, 
                product.Imagem, 
                product.Stock, 
                product.IsActive, 
                product.CategoryId);
        }
    }
}
