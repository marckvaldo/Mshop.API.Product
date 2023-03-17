using MShop.Application.UseCases.Category.Common;
using MShop.EndToEndTest.API.Common;
using MShop.EndToEndTest.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MShop.EndToEndTest.API.Categoria
{
    [Collection("Crud Category Collection")]
    [CollectionDefinition("Crud Category Collection", DisableParallelization = true)]
    public class CategoryAPITest : CategoryAPITestFixture
    {

        [Fact(DisplayName = "")]
        [Trait("","")]

        public async void CrudCategory()
        {
            var request = RequestCreate();
            var (response, outPut) = await apiClient.Post<CustomResponse<CategoryModelOutPut>>(Configuration.URL_API_CATEGORY, request);

            Assert.NotNull(request);
            Assert.Equal(System.Net.HttpStatusCode.OK, response?.StatusCode);
            Assert.NotNull(outPut);
            Assert.True(outPut.Success);
            Assert.Equal(outPut.Data.Name, request.Name);
            Assert.Equal(outPut.Data.IsActive, request.IsActive);
            
        }
    }
}
