using MShop.Application.UseCases.Product.Common;
using MShop.Business.Interface.Service;
using MShop.Core.Base;
using MShop.Core.Message;
using MShop.Repository.Interface;
using MShop.Core.Data;
using MShop.Core.DomainObject;

namespace MShop.Application.UseCases.Product.DeleteProduct
{
    public class DeleteProduct : BaseUseCase, IDeleteProduct
    {
        private readonly IProductRepository _productRespository;
        private readonly IImageRepository _imageRepository;
        private readonly IStorageService _storageService;
        private readonly IUnitOfWork _unitOfWork;
 
        public DeleteProduct(IProductRepository productRespository, 
            IImageRepository imageRepository,
            INotification notification,
            IStorageService storageService,
            IUnitOfWork unitOfWork):base(notification)
        {
            _productRespository = productRespository;
            _imageRepository = imageRepository;
            _storageService = storageService;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<ProductModelOutPut>> Handle(DeleteProductInPut request, CancellationToken cancellationToken)
        {
            var product = await _productRespository.GetById(request.Id);
            //NotifyExceptionIfNull(product, "Não foi possivel localizar a produto da base de dados!");
            if (NotifyErrorIfNull(product, "Não foi possivel localizar a produto da base de dados!"))
                return Result<ProductModelOutPut>.Error();

            var hasImages = await _imageRepository.Filter(x => x.ProductId == product!.Id);
            if(hasImages?.Count > 0)
            {
                Notify("Existe(m) Imagen(s) associada(s) a esse produto");
                return Result<ProductModelOutPut>.Error();
            }
                

            product!.ProductRemovedEvent();
            //NotifyExceptionIfNull(product.Events.Count == 0 ? null : product.Events, $" Não foi possivel registrar o event ProductDeletedEvent");
            if (NotifyErrorIfNull(product.Events.Count == 0 ? null : product.Events, $" Não foi possivel registrar o event ProductDeletedEvent"))
                return Result<ProductModelOutPut>.Error();

            await _productRespository.DeleteById(product, cancellationToken);
            await _unitOfWork.CommitAsync(cancellationToken);   

            if (product?.Thumb?.Path is not null) await _storageService.Delete(product.Thumb.Path);

            /*return new ProductModelOutPut(
                product!.Id, 
                product.Description,
                product.Name, 
                product.Price, 
                product.Thumb?.Path, 
                product.Stock, 
                product.IsActive, 
                product.CategoryId);*/

            return Result<ProductModelOutPut>.Success(ProductModelOutPut.FromProduct(product)); 
        }
    }
}
