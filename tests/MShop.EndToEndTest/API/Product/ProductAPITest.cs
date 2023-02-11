using UseCase = MShop.Application.UseCases.Product.Common;
using MShop.EndToEndTest.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using MShop.Application.UseCases.Product.Common;
using Microsoft.AspNetCore.Mvc;
using MShop.EndToEndTest.API.Product.Common;
using MShop.Repository.Context;
using Microsoft.EntityFrameworkCore;
using Bogus;
using Microsoft.VisualStudio.TestPlatform.Utilities;

namespace MShop.EndToEndTest.API.Product
{
    public class ProductAPITest : ProductAPITestFixture
    {
        private readonly RepositoryDbContext _dbContex;

        public ProductAPITest()
        {
            _dbContex = CreateDBContext();
        }

        [Fact(DisplayName = nameof(CreateProductAPI))]
        [Trait("EndToEnd/API","Product - Endpoints")]

        public async Task CreateProductAPI()
        {
            var request = RequestCreate();

            var (response, outPut) = await apiClient.Post<CustomResponse<ProductModelOutPut>>("/api/products", request);

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

            var (response, output) = await apiClient.Put<CustomResponse<ProductModelOutPut>>($"/api/Products/{request.Id}", request);

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

            var (response, output) = await apiClient.Delete<CustomResponse<ProductModelOutPut>>($"/api/products/{product.Id}");

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

            var (response, outPut) = await apiClient.Post<CustomResponse<ProductModelOutPut>>($"/api/products/update-stock/{product.Id}",new {product.Id, stock });

            var productDb = await Persistence.GetById(product.Id);

            Assert.NotNull(response);
            Assert.NotNull(outPut);
            Assert.NotNull(productDb);
            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(product.Name, outPut.Data.Name);
            Assert.Equal(product.Description, outPut.Data.Description);
            Assert.Equal(product.Price, outPut.Data.Price);
            Assert.Equal(product.Imagem, outPut.Data.Imagem);
            Assert.Equal(product.Stock, stock);

        }
    }
}
