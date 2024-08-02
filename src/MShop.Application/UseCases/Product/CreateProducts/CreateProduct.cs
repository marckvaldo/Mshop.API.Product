using MShop.Application.Common;
using MShop.Application.UseCases.Product.Common;
using MShop.Business.Events.Products;
using MShop.Business.Interface.Service;
using MShop.Core.Data;
using MShop.Core.DomainObject;
using MShop.Core.Message;
using MShop.Repository.Interface;

namespace MShop.Application.UseCases.Product.CreateProducts
{
    public class CreateProduct : Core.Base.BaseUseCase, ICreateProduct
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IStorageService _storageService;
        private readonly IUnitOfWork _unitOfWork;        

        public CreateProduct(IProductRepository productRepository, 
            INotification notification,
            ICategoryRepository categoryRepository,
            IStorageService storageService,
            IUnitOfWork unitOfWork) : base(notification)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _storageService = storageService;  
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<ProductModelOutPut>> Handle(CreateProductInPut request, CancellationToken cancellationToken)
        {
            var product = new Business.Entity.Product(
                    request.Description,
                    request.Name,
                    request.Price,
                    request.CategoryId,
                    request.Stock,
                    request.IsActive
                );

            if (!product.IsValid(Notifications))
                return Result<ProductModelOutPut>.Error();

            var category = await _categoryRepository.GetById(product.CategoryId);
            //NotifyExceptionIfNull(category, $"Categoria {product.CategoryId} não encontrada");
            if(NotifyErrorIfNull(category, $"Categoria {product.CategoryId} não encontrada"))
                return Result<ProductModelOutPut>.Error();

            try
            {
                await UploadImage(request, product);

                product.ProductCreatedEvent(new ProductCreatedEvent(
                product.Id,
                product.Description,
                product.Name,
                product.Price,
                product.Stock,
                product.IsActive,
                product.CategoryId,
                category.Name,
                product.Thumb?.Path,
                product.IsSale));

                //NotifyExceptionIfNull(product.Events.Count == 0 ? null : product.Events, $" Não foi possivel registrar o event ProductCreatedEvent");
                if(NotifyErrorIfNull(product.Events.Count == 0 ? null : product.Events, $" Não foi possivel registrar o event ProductCreatedEvent"))
                    return Result<ProductModelOutPut>.Error();

                await _productRepository.Create(product,cancellationToken);
                await _unitOfWork.CommitAsync(cancellationToken);


                var produtoOutPut =  new ProductModelOutPut(
                        product.Id,
                        product.Description,
                        product.Name,
                        product.Price,
                        product.Thumb?.Path,
                        product.Stock,
                        product.IsActive,
                        product.CategoryId
                    );

                return Result<ProductModelOutPut>.Success(produtoOutPut);
            }
            catch(Exception)
            {
                if(product?.Thumb?.Path is not null) 
                    await _storageService.Delete(product.Thumb.Path);
                throw;
            }
        }

        private async Task UploadImage(CreateProductInPut request, Business.Entity.Product product)
        {
            if (string.IsNullOrEmpty(request.Thumb?.FileStremBase64.Trim()))
                return;
           

            var thumbFile = Helpers.Base64ToStream(request.Thumb.FileStremBase64);
            var urlThumb = await _storageService.Upload($"{product.Id}-thumb.{thumbFile.Extension}", thumbFile.FileStrem);
            product.UpdateThumb(urlThumb);
            
        }
    }
}
