using UseCase = MShop.Application.UseCases.Product.Common;
using MShop.EndToEndTest.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using MShop.Application.UseCases.Product.Common;

namespace MShop.EndToEndTest.API.Product
{
    public class CreateProductApiTest : CreateProductAPITestFixture
    {
        [Fact(DisplayName = nameof(CreateProductAPI))]
        [Trait("EndToEnd/API","Category - Endpoints")]

        public async Task CreateProductAPI()
        {
            var request = Request();

            var (respose, outPut) = await _apiClient.Post<ProductModelOutPut>("/product", request);

            Assert.NotNull(respose);
            Assert.Equal(respose!.StatusCode.ToString(), StatusCodes.Status200OK.ToString());
            Assert.NotNull(outPut);
            Assert.Equal(outPut.Name, request.Name);
            Assert.Equal(outPut.Imagem, request.Imagem);   
            Assert.Equal(outPut.Description, request.Description);  
            Assert.Equal(outPut.Price,request.Price);   
            Assert.Equal(outPut.IsActive,request.IsActive);

            var dbProduct = await Persistence.GetById(outPut.Id);

            Assert.NotNull(dbProduct);
            Assert.Equal(dbProduct.Name, request.Name);
            Assert.Equal(dbProduct.Imagem, request.Imagem);
            Assert.Equal(dbProduct.Description, request.Description);
            Assert.Equal(dbProduct.Price, request.Price);
            Assert.Equal(dbProduct.IsActive, request.IsActive);

        }
    }
}
