using MShop.Application.UseCases.Images.Common;
using MShop.EndToEndTest.API.Common;
using MShop.EndToEndTest.Common;

namespace MShop.EndToEndTest.API.Images
{
    [Collection("Crud Image Collection")]
    [CollectionDefinition("Crud Image Collection", DisableParallelization = true)]
    public class ImageAPITest : ImageAPITestFixture
    {
        

        [Fact(DisplayName = nameof(CreateImage))]
        [Trait("EndToEnd/API", "Image - Endpoints")]
        
        public async void CreateImage()
        {
            var request = await FakeRequest();
            var (response, outPut) = await apiClient.Post<CustomResponse<ListImageOutPut>>(Configuration.URL_API_IMAGE, request);

            Assert.NotNull(request);
            Assert.NotNull(request.Images);
            Assert.Equal(System.Net.HttpStatusCode.OK, response?.StatusCode);
            Assert.NotNull(outPut);
            Assert.True(outPut.Success);
            Assert.Equal(outPut.Data.Images.Count(), request.Images.Count());
            Assert.Equal(outPut.Data.ProductId, request.ProductId);
            
        }



        [Fact(DisplayName = nameof(DeleteImage))]
        [Trait("EndToEnd/API", "Image - Endpoints")]

        public async void DeleteImage()
        {
            var images = ListImage();
            await _imagePersistence.CreateList(images);
            var request = images.First();

            var (response, outPut) = await apiClient.Delete<CustomResponse<ImageOutPut>>($"{Configuration.URL_API_IMAGE}{request.Id}");

            Assert.NotNull(request);
            Assert.NotNull(images);
            Assert.Equal(System.Net.HttpStatusCode.OK, response?.StatusCode);
            Assert.NotNull(outPut);
            Assert.True(outPut.Success);
            Assert.Equal(outPut.Data.Image.Image, request.FileName);
            Assert.Equal(outPut.Data.ProductId, request.ProductId);
        }


        [Fact(DisplayName = nameof(GetImage))]
        [Trait("EndToEnd/API", "Image - Endpoints")]

        public async void GetImage()
        {
            var images = ListImage();
            await _imagePersistence.CreateList(images);
            var request = images.First();

            var (response, outPut) = await apiClient.Get<CustomResponse<ImageOutPut>>($"{Configuration.URL_API_IMAGE}{request.Id}");

            Assert.NotNull(request);
            Assert.NotNull(images);
            Assert.Equal(System.Net.HttpStatusCode.OK, response?.StatusCode);
            Assert.NotNull(outPut);
            Assert.True(outPut.Success);
            Assert.Equal(outPut.Data.Image.Image, request.FileName);
            Assert.Equal(outPut.Data.ProductId, request.ProductId);
        }


        [Fact(DisplayName = nameof(ListImageAPI))]
        [Trait("EndToEnd/API", "Image - Endpoints")]

        public async void ListImageAPI()
        {
            var images = ListImage();
            await _imagePersistence.CreateList(images);
            var request = images.First();

            var (response, outPut) = await apiClient.Get<CustomResponse<ListImageOutPut>>($"{Configuration.URL_API_IMAGE}list-Images-by-id-production/{request.ProductId}");

            Assert.NotNull(request);
            Assert.NotNull(images);
            Assert.Equal(System.Net.HttpStatusCode.OK, response?.StatusCode);
            Assert.NotNull(outPut);
            Assert.True(outPut.Success);
            Assert.Equal(outPut.Data.Images.Count(), images.Count());
            Assert.Equal(outPut.Data.ProductId, request.ProductId);

        }
    }
}
