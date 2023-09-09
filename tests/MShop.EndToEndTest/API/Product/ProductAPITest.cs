using MShop.Application.UseCases.Product.Common;
using MShop.Application.UseCases.Product.ListProducts;
using MShop.Business.Entity;
using MShop.Business.Enum.Paginated;
using MShop.Business.Events.Products;
using MShop.EndToEndTest.API.Common;
using MShop.EndToEndTest.API.Product.Common;
using MShop.EndToEndTest.Common;

namespace MShop.EndToEndTest.API.Product
{
    [Collection("Crud Products Collection")]
    [CollectionDefinition("Crud Products Collection", DisableParallelization = true)]

    public class ProductAPITest : ProductAPITestFixture, IDisposable
    {
        public ProductAPITest()
        {
            
        }

        [Fact(DisplayName = nameof(CreateProductAPI))]
        [Trait("EndToEnd/API","Product - Endpoints")]
        public async Task CreateProductAPI()
        {
            var request = await RequestCreate();
            SetupRabbitMQ(); 
            var (response, outPut) = await apiClient.Post<CustomResponse<ProductModelOutPut>>(Configuration.URL_API_PRODUCT, request);

            Assert.NotNull(response);
            Assert.Equal(System.Net.HttpStatusCode.OK, response!.StatusCode) ;
            Assert.NotNull(outPut);
            Assert.True(outPut.Success);
            Assert.Equal(outPut.Data.Name, request.Name);
            Assert.Equal(outPut.Data.Description, request.Description);  
            Assert.Equal(outPut.Data.Price,request.Price);   
            Assert.Equal(outPut.Data.IsActive,request.IsActive);

            var dbProduct = await Persistence.GetById(outPut.Data.Id);

            Assert.NotNull(dbProduct);
            Assert.Equal(dbProduct.Name, request.Name);            
            Assert.Equal(dbProduct.Description, request.Description);
            Assert.Equal(dbProduct.Price, request.Price);
            Assert.Equal(dbProduct.IsActive, request.IsActive);


            Thread.Sleep(1000);
            var (@event, reamainingMessages) = ReadMessageFromRabbitMQAck<ProductCreatedEvent>();
            Assert.Equal(0, ((int)reamainingMessages));
            Assert.NotNull(@event);
            Assert.Equal(@event.ProductId, dbProduct.Id);
            Assert.Equal(@event.Name, dbProduct.Name);
            Assert.Equal(@event.Price, dbProduct.Price);
            Assert.Equal(@event.Thumb.Path, dbProduct.Thumb.Path);
            Assert.Equal(@event.CategoryId, dbProduct.CategoryId);
            Assert.Equal(@event.Description, dbProduct.Description);
            Assert.Equal(@event.IsActive, dbProduct.IsActive);  
            Assert.Equal(@event.IsSale, dbProduct.IsSale);
            Assert.Equal(@event.Stock, dbProduct.Stock);
            Assert.NotEqual(@event.OccuredOn, default);
            TearDownRabbitMQ();

        }


        [Fact(DisplayName = nameof(ShoudSendTheMessageToDeadLetterQueueWhenCreateProduct))]
        [Trait("EndToEnd/API", "Product - Endpoints")]
        public async Task ShoudSendTheMessageToDeadLetterQueueWhenCreateProduct()
        {
            var request = await RequestCreate();
            SetupRabbitMQ();
            var (response, outPut) = await apiClient.Post<CustomResponse<ProductModelOutPut>>(Configuration.URL_API_PRODUCT, request);

            Assert.NotNull(response);
            Assert.Equal(System.Net.HttpStatusCode.OK, response!.StatusCode);
            Assert.NotNull(outPut);
            Assert.True(outPut.Success);
            Assert.Equal(outPut.Data.Name, request.Name);
            Assert.Equal(outPut.Data.Description, request.Description);
            Assert.Equal(outPut.Data.Price, request.Price);
            Assert.Equal(outPut.Data.IsActive, request.IsActive);

            var dbProduct = await Persistence.GetById(outPut.Data.Id);

            Assert.NotNull(dbProduct);
            Assert.Equal(dbProduct.Name, request.Name);
            Assert.Equal(dbProduct.Description, request.Description);
            Assert.Equal(dbProduct.Price, request.Price);
            Assert.Equal(dbProduct.IsActive, request.IsActive);


            Thread.Sleep(1000);
            ReadMessageFromRabbitMQNack<ProductCreatedEvent>();
            var (@event, reamainingMessages) = ReadMessageFromRabbitMQDeadLetterQueue<ProductCreatedEvent>();

            Assert.Equal(0, ((int)reamainingMessages));
            Assert.NotNull(@event);
            Assert.Equal(@event.ProductId, dbProduct.Id);
            Assert.Equal(@event.Name, dbProduct.Name);
            Assert.Equal(@event.Price, dbProduct.Price);
            Assert.Equal(@event.Thumb.Path, dbProduct.Thumb.Path);
            Assert.Equal(@event.CategoryId, dbProduct.CategoryId);
            Assert.Equal(@event.Description, dbProduct.Description);
            Assert.Equal(@event.IsActive, dbProduct.IsActive);
            Assert.Equal(@event.IsSale, dbProduct.IsSale);
            Assert.Equal(@event.Stock, dbProduct.Stock);
            Assert.NotEqual(@event.OccuredOn, default);
            TearDownRabbitMQ();

        }


        [Fact(DisplayName = nameof(UpdateProduct))]
        [Trait("EndToEnd/API", "Product - Endpoints")]
        public async Task UpdateProduct()
        {
            SetupRabbitMQ();
            var request = await RequestUpdate();
            var product = await Faker();
            request.Id = product.Id;
            
            
            Persistence.Create(product);

            var (response, output) = await apiClient.Put<CustomResponse<ProductModelOutPut>>(
                $"{Configuration.URL_API_PRODUCT}{request.Id}", 
                request);

            var persistence = await Persistence.GetById(product.Id);

            Assert.NotNull(response);
            Assert.NotNull(persistence);
            Assert.NotNull(output);
            Assert.Equal(System.Net.HttpStatusCode.OK, response!.StatusCode);
            Assert.True(output.Success);
            Assert.Equal(persistence.Name, output.Data.Name);
            Assert.Equal(persistence.Description, output.Data.Description);
            Assert.Equal(persistence.Id, output.Data.Id);
            //Assert.Equal(persistence.Thumb.Path, output.Data.Imagem);
            Assert.Equal(persistence.Price, output.Data.Price);

            var (@event, reamainingMessages)  = ReadMessageFromRabbitMQAck<ProductUpdatedEvent>();
            Assert.Equal(0, ((int)reamainingMessages));
            Assert.NotNull(@event);
            Assert.Equal(@event.ProductId, persistence.Id);
            Assert.Equal(@event.Name, persistence.Name);
            Assert.Equal(@event.Price, persistence.Price);
            Assert.Equal(@event.Thumb.Path, persistence.Thumb.Path);
            Assert.Equal(@event.CategoryId, persistence.CategoryId);
            Assert.Equal(@event.Description, persistence.Description);
            Assert.Equal(@event.IsActive, persistence.IsActive);
            Assert.Equal(@event.IsSale, persistence.IsSale);
            Assert.Equal(@event.Stock, persistence.Stock);
            Assert.NotEqual(@event.OccuredOn, default);
            TearDownRabbitMQ();
        }


        [Fact(DisplayName = nameof(ShoudSendTheMessageToDeadLetterQueueWhenUpdateProduct))]
        [Trait("EndToEnd/API", "Product - Endpoints")]
        public async Task ShoudSendTheMessageToDeadLetterQueueWhenUpdateProduct()
        {
            SetupRabbitMQ();
            var request = await RequestUpdate();
            var product = await Faker();
            request.Id = product.Id;


            Persistence.Create(product);

            var (response, output) = await apiClient.Put<CustomResponse<ProductModelOutPut>>(
                $"{Configuration.URL_API_PRODUCT}{request.Id}",
                request);

            var persistence = await Persistence.GetById(product.Id);

            Assert.NotNull(response);
            Assert.NotNull(persistence);
            Assert.NotNull(output);
            Assert.Equal(System.Net.HttpStatusCode.OK, response!.StatusCode);
            Assert.True(output.Success);
            Assert.Equal(persistence.Name, output.Data.Name);
            Assert.Equal(persistence.Description, output.Data.Description);
            Assert.Equal(persistence.Id, output.Data.Id);
            //Assert.Equal(persistence.Thumb.Path, output.Data.Imagem);
            Assert.Equal(persistence.Price, output.Data.Price);

            Thread.Sleep(1000);
            ReadMessageFromRabbitMQNack<ProductUpdatedEvent>();
            Thread.Sleep(1000);
            var (@event, reamainingMessages) = ReadMessageFromRabbitMQDeadLetterQueue<ProductUpdatedEvent>();
            Thread.Sleep(1000);

            Assert.Equal(0, ((int)reamainingMessages));
            Assert.NotNull(@event);
            Assert.Equal(@event.ProductId, persistence.Id);
            Assert.Equal(@event.Name, persistence.Name);
            Assert.Equal(@event.Price, persistence.Price);
            Assert.Equal(@event.Thumb.Path, persistence.Thumb.Path);
            Assert.Equal(@event.CategoryId, persistence.CategoryId);
            Assert.Equal(@event.Description, persistence.Description);
            Assert.Equal(@event.IsActive, persistence.IsActive);
            Assert.Equal(@event.IsSale, persistence.IsSale);
            Assert.Equal(@event.Stock, persistence.Stock);
            Assert.NotEqual(@event.OccuredOn, default);
            TearDownRabbitMQ();
        }


        [Fact(DisplayName = nameof(DeleteProduct))]
        [Trait("EndToEnd/API", "Product - Endpoints")]
        public async Task DeleteProduct()
        {
            var product = await Faker();
            Persistence.Create(product);
            SetupRabbitMQ();

            var (response, output) = await apiClient.Delete<CustomResponse<ProductModelOutPut>>($"{Configuration.URL_API_PRODUCT}{product.Id}");

            var dbProduct = await Persistence.GetById(product.Id);

            Assert.NotNull(response);
            Assert.NotNull(output);
            Assert.True(output.Success);
            Assert.Equal(System.Net.HttpStatusCode.OK, response!.StatusCode);
            Assert.Equal(product.Id, output.Data.Id);
            Assert.Equal(product.Name, output.Data.Name);   
            Assert.Equal(product.Price, output.Data.Price); 
            Assert.Equal(product.Description, output.Data.Description);
            Assert.Null(dbProduct);

            var (@event, reamainingMessages) = ReadMessageFromRabbitMQAck<ProductRemovedEvent>();
            Assert.Equal(0,(int)reamainingMessages);
            Assert.Equal(@event.ProductId, output.Data.Id);
            TearDownRabbitMQ();
        }


        [Fact(DisplayName = nameof(ShoudSendTheMessageToDeadLetterQueueWhenDeleteProduct))]
        [Trait("EndToEnd/API", "Product - Endpoints")]
        public async Task ShoudSendTheMessageToDeadLetterQueueWhenDeleteProduct()
        {
            var product = await Faker();
            Persistence.Create(product);
            SetupRabbitMQ();

            var (response, output) = await apiClient.Delete<CustomResponse<ProductModelOutPut>>($"{Configuration.URL_API_PRODUCT}{product.Id}");

            var dbProduct = await Persistence.GetById(product.Id);

            Assert.NotNull(response);
            Assert.NotNull(output);
            Assert.True(output.Success);
            Assert.Equal(System.Net.HttpStatusCode.OK, response!.StatusCode);
            Assert.Equal(product.Id, output.Data.Id);
            Assert.Equal(product.Name, output.Data.Name);
            Assert.Equal(product.Price, output.Data.Price);
            Assert.Equal(product.Description, output.Data.Description);
            Assert.Null(dbProduct);

            Thread.Sleep(1000);
            ReadMessageFromRabbitMQNack<ProductRemovedEvent>();
            Thread.Sleep(1000);
            var (@event, reamainingMessages) = ReadMessageFromRabbitMQDeadLetterQueue<ProductRemovedEvent>();
            Thread.Sleep(1000);
            Assert.Equal(0, (int)reamainingMessages);
            Assert.Equal(@event.ProductId, output.Data.Id);
            TearDownRabbitMQ();
        }


        [Fact(DisplayName = nameof(UpdateProductStock))]
        [Trait("EndToEnd/API", "Product - Endpoints")]
        public async Task UpdateProductStock()
        {
            var product = await FakerImage();
            Persistence.Create(product);
            var productStock = await FakerImage();
            var stock = productStock.Stock;
            SetupRabbitMQ();

            var (response, outPut) = await apiClient.Post<CustomResponse<ProductModelOutPut>>($"{Configuration.URL_API_PRODUCT}update-stock/{product.Id}",new {product.Id, stock });

            var productDb = await Persistence.GetById(product.Id);

            Assert.NotNull(response);
            Assert.NotNull(outPut);
            Assert.NotNull(productDb);
            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(product.Name, outPut.Data.Name);
            Assert.Equal(product.Description, outPut.Data.Description);
            Assert.Equal(product.Price, outPut.Data.Price);
            Assert.Equal(productDb.Stock, stock);

            Thread.Sleep(1000);
            var (@event, reamainingMessages) = ReadMessageFromRabbitMQAck<ProductUpdatedEvent>();
            Assert.Equal(0, ((int)reamainingMessages));
            Assert.NotNull(@event);
            Assert.Equal(@event.ProductId, productDb.Id);
            Assert.Equal(@event.Name, productDb.Name);
            Assert.Equal(@event.Price, productDb.Price);
            Assert.Equal(@event.Thumb.Path, productDb.Thumb.Path);
            Assert.Equal(@event.CategoryId, productDb.CategoryId);
            Assert.Equal(@event.Description, productDb.Description);
            Assert.Equal(@event.IsActive, productDb.IsActive);
            Assert.Equal(@event.IsSale, productDb.IsSale);
            Assert.Equal(@event.Stock, productDb.Stock);
            Assert.NotEqual(@event.OccuredOn, default);
            TearDownRabbitMQ();

        }


        [Theory(DisplayName = nameof(SholdReturnErrorWhenCantCreatePoduct))]
        [Trait("EndToEnd/API", "Product - Endpoints")]
        [InlineData(0)]
        [InlineData(-1)]
        public async Task SholdReturnErrorWhenCantCreatePoduct(decimal price)
        {
            var request = await RequestCreate();
            request.Price = price;
            SetupRabbitMQ();

            var (response, outPut) = await apiClient.Post<CustomResponseErro> (Configuration.URL_API_PRODUCT, request);

            Assert.NotNull(response);
            Assert.Equal(System.Net.HttpStatusCode.BadRequest, response!.StatusCode);
            Assert.NotNull(outPut);
            Assert.True(outPut.Errors.Count > 0);

            var (@event, reamainingMessages) = ReadMessageFromRabbitMQAck<ProductCreatedEvent>();
            Assert.Null(@event);
            TearDownRabbitMQ();
        }


        [Theory(DisplayName = nameof(SholdReturnErrorWhenCantUpdatePoduct))]
        [Trait("EndToEnd/API", "Product - Endpoints")]
        [InlineData(0)]
        [InlineData(-1)]
        public async Task SholdReturnErrorWhenCantUpdatePoduct(decimal price)
        {
            var request =  await RequestUpdate();
            request.Price = price;
            SetupRabbitMQ();

            var (response, outPut) = await apiClient.Post<CustomResponseErro>(Configuration.URL_API_PRODUCT, request);

            Assert.NotNull(response);
            Assert.Equal(System.Net.HttpStatusCode.BadRequest, response!.StatusCode);
            Assert.NotNull(outPut);
            Assert.True(outPut.Errors.Count > 0);

            var (@event, reamainingMessages) = ReadMessageFromRabbitMQAck<ProductCreatedEvent>();
            Assert.Null(@event);
            TearDownRabbitMQ();
        }


        [Fact(DisplayName = nameof(GetProductById))]
        [Trait("EndToEnd/API", "Product - Endpoints")]
        public async Task GetProductById()
        {
            var products = await GetProducts(20);
            await Persistence.CreateList(products);
            var product = products[3];
           
            var (response, outPut) = await apiClient.Get<CustomResponse<ProductModelOutPut>>($"{Configuration.URL_API_PRODUCT}{product.Id}");

            Assert.NotNull(response);
            Assert.Equal(System.Net.HttpStatusCode.OK, response!.StatusCode);
            Assert.NotNull(outPut);
            Assert.True(outPut.Success);
            Assert.True(outPut.Data.Id == product.Id);  
        }


        [Fact(DisplayName = nameof(ListProduct))]
        [Trait("EndToEnd/API", "Product - Endpoints")]
        public async Task ListProduct()
        {
            var products = await GetProducts(20);
            await Persistence.CreateList(products);

            var productDbBefore = await Persistence.List();          
            var (response, outPut) = await apiClient.Get<CustomResponsePaginated<ProductModelOutPut>>($"{Configuration.URL_API_PRODUCT}list-products/");

            Assert.NotNull(response);
            Assert.Equal(System.Net.HttpStatusCode.OK, response!.StatusCode);
            Assert.NotNull(outPut);
            Assert.True(outPut.Success);
            Assert.True(productDbBefore.Count() == outPut.Data.Total);
            Assert.True(outPut.Data.PerPage == 15);
            Assert.True(outPut.Data.Page == 1);

            foreach (var item in outPut.Data.Itens)
            {
                var expectItem = products.FirstOrDefault(x => x.Id == item.Id);

                Assert.NotNull(expectItem);
                Assert.Equal(expectItem.Name, item.Name);
                Assert.Equal(expectItem.Description, item.Description);
                Assert.Equal(expectItem.Price, item.Price);
                //Assert.Equal(expectItem.Thumb?.Path, item.Imagem);
            }

        }


        [Fact(DisplayName = nameof(ListProductWhenItemsEmptyDefault))]
        [Trait("EndToEnd/API", "Product - Endpoints")]
        public async Task ListProductWhenItemsEmptyDefault()
        {
            var (response, outPut) = await apiClient.Get<CustomResponsePaginated<ProductModelOutPut>>($"{Configuration.URL_API_PRODUCT}list-products/");

            Assert.NotNull(response);
            Assert.Equal(System.Net.HttpStatusCode.OK, response!.StatusCode);
            Assert.NotNull(outPut);
            Assert.True(outPut.Success);
            Assert.True(outPut.Data.PerPage == 15);
            Assert.True(outPut.Data.Page == 1);
            Assert.True(outPut.Data.Itens.Count() == 0);
        }


        [Theory(DisplayName = nameof(ListProductWithPaginated))]
        [Trait("EndToEnd/API", "Product - Endpoints")]
        [InlineData(10, 1, 10, 10)]
        [InlineData(17, 2, 10, 7)]
        [InlineData(17, 3, 10, 0)]
        public async Task ListProductWithPaginated(int quantityProduct, int page, int perPage, int expectedQuantityItems)
        {
            await BuildCategory();
            var products = await GetProducts(quantityProduct);
            await Persistence.CreateList(products);

            var request = new ListProductInPut(page, perPage, "", "", SearchOrder.Desc);
            var (response, outPut) = await apiClient.Get<CustomResponsePaginated<ProductModelOutPut>>($"{Configuration.URL_API_PRODUCT}list-products/", request);

            Assert.NotNull(response);
            Assert.Equal(System.Net.HttpStatusCode.OK, response!.StatusCode);
            Assert.NotNull(outPut);
            Assert.True(outPut.Success);
            Assert.True(outPut.Data.PerPage == perPage);
            Assert.True(outPut.Data.Page == page);
            Assert.True(outPut.Data.Itens.Count() == expectedQuantityItems);

            foreach (var item in outPut.Data.Itens)
            {
                var expectItem = products.FirstOrDefault(x => x.Id == item.Id);

                Assert.NotNull(expectItem);
                Assert.Equal(expectItem.Name, item.Name);
                Assert.Equal(expectItem.Description, item.Description);
                Assert.Equal(expectItem.Price, item.Price);
                //Assert.Equal(expectItem.Thumb.Path, item.Imagem);
            }
        }

        public void Dispose()
        {
            //TearDownRabbitMQ();
        }

        /*
        [Fact(DisplayName = nameof(ListProductPromotions))]
        [Trait("EndToEnd/API", "Product - Endpoints")]
        public async void ListProductPromotions()
        {
            var products = await GetProducts(5);
            await Persistence.CreateList(products);

            var (response, outPut) = await apiClient.Get<CustomResponse<List<ProductModelOutPut>>>($"{Configuration.URL_API_PRODUCT}list-products-promotions");

            Assert.NotNull(response);
            Assert.Equal(System.Net.HttpStatusCode.OK, response!.StatusCode);
            Assert.NotNull(outPut);
            Assert.True(outPut.Success);

            foreach(var item in outPut.Data)
            {
                var expectItem = products.FirstOrDefault(p=>p.Id == item.Id);
                Assert.NotNull(expectItem);
                Assert.Equal(expectItem.Name, item.Name);   
                Assert.Equal(expectItem.Description,expectItem.Description);
                Assert.Equal(expectItem.Thumb, expectItem.Thumb);
                Assert.Equal(expectItem.Price, expectItem.Price);
                Assert.Equal(expectItem.Activate, expectItem.Activate); 
            }
        }


        [Fact(DisplayName = nameof(SholdReturnErrorWhenCantGetProductPromotion))]
        [Trait("EndToEnd/API", "Product - Endpoints")]
        public async void SholdReturnErrorWhenCantGetProductPromotion()
        {
            await ProductPersistenceCache.DeleteKey("promocao");
            var (response, outPut) = await apiClient.Get<CustomResponseErro>($"{Configuration.URL_API_PRODUCT}list-products-promotions");

            Assert.NotNull(response);
            Assert.Equal(System.Net.HttpStatusCode.BadRequest, response!.StatusCode);
            Assert.NotNull(outPut);
            Assert.False(outPut.Success);
        }
        */
    }
}
