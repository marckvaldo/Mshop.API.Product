using UseCase = MShop.Application.UseCases.Product.Common;
using MShop.EndToEndTest.Common;
using System;
using MShop.Application.UseCases.Product.Common;
using MShop.Application.UseCases.Product.ListProducts;
using MShop.EndToEndTest.API.Product.Common;
using MShop.Repository.Context;
using MShop.Business.Enum.Paginated;

namespace MShop.EndToEndTest.API.Product
{
    public class ProductAPITest : ProductAPITestFixture
    {
        //private readonly RepositoryDbContext _dbContex;

        public ProductAPITest()
        {
            //_dbContex = CreateDBContext();
        }

        [Fact(DisplayName = nameof(CreateProductAPI))]
        [Trait("EndToEnd/API","Product - Endpoints")]
        public async Task CreateProductAPI()
        {
            var request = RequestCreate();

            var (response, outPut) = await apiClient.Post<CustomResponse<ProductModelOutPut>>(Configuration.URL_API_PRODUCT, request);

            Assert.NotNull(response);
            Assert.Equal(System.Net.HttpStatusCode.OK, response!.StatusCode) ;
            Assert.NotNull(outPut);
            Assert.True(outPut.Success);
            Assert.Equal(outPut.Data.Name, request.Name);
            Assert.Equal(outPut.Data.Imagem, request.Imagem);   
            Assert.Equal(outPut.Data.Description, request.Description);  
            Assert.Equal(outPut.Data.Price,request.Price);   
            Assert.Equal(outPut.Data.IsActive,request.IsActive);

            var dbProduct = await Persistence.GetById(outPut.Data.Id);

            Assert.NotNull(dbProduct);
            Assert.Equal(dbProduct.Name, request.Name);
            Assert.Equal(dbProduct.Imagem, request.Imagem);
            Assert.Equal(dbProduct.Description, request.Description);
            Assert.Equal(dbProduct.Price, request.Price);
            Assert.Equal(dbProduct.IsActive, request.IsActive);  
        }


        [Fact(DisplayName = nameof(UpdateProduct))]
        [Trait("EndToEnd/API", "Product - Endpoints")]
        public async Task UpdateProduct()
        {
            var request = RequestUpdate();
            var product = Faker();
            request.Id = product.Id;

            Persistence.Create(product);

            var (response, output) = await apiClient.Put<CustomResponse<ProductModelOutPut>>($"{Configuration.URL_API_PRODUCT}{request.Id}", request);

            var persistence = await Persistence.GetById(product.Id);

            Assert.NotNull(response);
            Assert.NotNull(persistence);
            Assert.NotNull(output);
            Assert.True(output.Success);
            Assert.Equal(System.Net.HttpStatusCode.OK, response!.StatusCode);
            Assert.Equal(persistence.Name, output.Data.Name);
            Assert.Equal(persistence.Description, output.Data.Description);
            Assert.Equal(persistence.Id, output.Data.Id);
            Assert.Equal(persistence.Imagem, output.Data.Imagem);
            Assert.Equal(persistence.Price, output.Data.Price);
        }

        
        [Fact(DisplayName = nameof(DeleteProduct))]
        [Trait("EndToEnd/API", "Product - Endpoints")]
        public async Task DeleteProduct()
        {
            var product = Faker();
            Persistence.Create(product);

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
            
           
        }


        [Fact(DisplayName = nameof(UpdateProductStock))]
        [Trait("EndToEnd/API", "Product - Endpoints")]
        public async Task UpdateProductStock()
        {
            var product = Faker();
            Persistence.Create(product);
            var stock = Faker().Stock;

            var (response, outPut) = await apiClient.Post<CustomResponse<ProductModelOutPut>>($"{Configuration.URL_API_PRODUCT}update-stock/{product.Id}",new {product.Id, stock });

            var productDb = await Persistence.GetById(product.Id);

            Assert.NotNull(response);
            Assert.NotNull(outPut);
            Assert.NotNull(productDb);
            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(product.Name, outPut.Data.Name);
            Assert.Equal(product.Description, outPut.Data.Description);
            Assert.Equal(product.Price, outPut.Data.Price);
            Assert.Equal(product.Imagem, outPut.Data.Imagem);
            Assert.Equal(productDb.Stock, stock);

        }


        [Theory(DisplayName = nameof(SholdReturnErrorWhenCantCreatePoduct))]
        [Trait("EndToEnd/API", "Product - Endpoints")]
        [InlineData(0)]
        [InlineData(-1)]
        public async Task SholdReturnErrorWhenCantCreatePoduct(decimal price)
        {
            var request = RequestCreate();
            request.Price = price;

            var (response, outPut) = await apiClient.Post<CustomResponseErro> (Configuration.URL_API_PRODUCT, request);

            Assert.NotNull(response);
            Assert.Equal(System.Net.HttpStatusCode.BadRequest, response!.StatusCode);
            Assert.NotNull(outPut);
            Assert.True(outPut.Errors.Count > 0);
        }


        [Theory(DisplayName = nameof(SholdReturnErrorWhenCantUpdatePoduct))]
        [Trait("EndToEnd/API", "Product - Endpoints")]
        [InlineData(0)]
        [InlineData(-1)]
        public async Task SholdReturnErrorWhenCantUpdatePoduct(decimal price)
        {
            var request = RequestUpdate();
            request.Price = price;

            var (response, outPut) = await apiClient.Post<CustomResponseErro>(Configuration.URL_API_PRODUCT, request);

            Assert.NotNull(response);
            Assert.Equal(System.Net.HttpStatusCode.BadRequest, response!.StatusCode);
            Assert.NotNull(outPut);
            Assert.True(outPut.Errors.Count > 0);
        }


        [Fact(DisplayName = nameof(GetProductById))]
        [Trait("EndToEnd/API", "Product - Endpoints")]
        public async void GetProductById()
        {
            var products = GetProducts(20);
            Persistence.CreateList(products);
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
        public async void ListProduct()
        {
            var products = GetProducts(20);
            Persistence.CreateList(products);

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
                Assert.Equal(expectItem.Imagem, item.Imagem);
            }

        }


        [Fact(DisplayName = nameof(ListProductWhenItemsEmptyDefault))]
        [Trait("EndToEnd/API", "Product - Endpoints")]
        public async void ListProductWhenItemsEmptyDefault()
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
        public async void ListProductWithPaginated(int quantityProduct, int page, int perPage, int expectedQuantityItems)
        {
            var products = GetProducts(quantityProduct);
            Persistence.CreateList(products);

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
                Assert.Equal(expectItem.Imagem, item.Imagem);
            }
        }


        [Fact(DisplayName = nameof(ListProductPromotions))]
        [Trait("EndToEnd/API", "Product - Endpoints")]
        public async void ListProductPromotions()
        {
            var products = GetProducts(5);
            Persistence.CreateList(products);

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
                Assert.Equal(expectItem.Imagem, expectItem.Imagem);
                Assert.Equal(expectItem.Price, expectItem.Price);
                Assert.Equal(expectItem.Activate, expectItem.Activate); 
            }
        }


        [Fact(DisplayName = nameof(SholdReturnErrorWhenCantGetProductPromotion))]
        [Trait("EndToEnd/API", "Product - Endpoints")]
        public async void SholdReturnErrorWhenCantGetProductPromotion()
        {

            var (response, outPut) = await apiClient.Get<CustomResponseErro>($"{Configuration.URL_API_PRODUCT}list-products-promotions");

            Assert.NotNull(response);
            Assert.Equal(System.Net.HttpStatusCode.BadRequest, response!.StatusCode);
            Assert.NotNull(outPut);
            Assert.False(outPut.Success);

            
        }
    }
}
