using MShop.Application.UseCases.Category.Common;
using MShop.Application.UseCases.Category.ListCategorys;
using MShop.Business.Enum.Paginated;
using MShop.EndToEndTest.API.Common;
using MShop.EndToEndTest.Common;

namespace MShop.EndToEndTest.API.Categoria
{
    [Collection("Crud Category Collection")]
    [CollectionDefinition("Crud Category Collection", DisableParallelization = true)]
    public class CategoryAPITest : CategoryAPITestFixture, IDisposable
    {

        [Fact(DisplayName = nameof(CreateCategory))]
        [Trait("EndToEnd/API", "Category - Endpoints")]

        public async void CreateCategory()
        {
            var request = RequestCreate();
            var (response, outPut) = await apiClient.Post<CustomResponse<CategoryModelOutPut>>(Configuration.URL_API_CATEGORY, request);

            Assert.NotNull(request);
            Assert.Equal(System.Net.HttpStatusCode.OK, response?.StatusCode);
            Assert.NotNull(outPut);
            Assert.True(outPut.Success);
            Assert.Equal(outPut.Data.Name, request.Name);
            Assert.Equal(outPut.Data.IsActive, request.IsActive);

            var dbCategory = await Persistence.GetById(outPut.Data.Id);

            Assert.NotNull(dbCategory);
            Assert.Equal(dbCategory.Name, request.Name);
            Assert.Equal(dbCategory.IsActive, request.IsActive);

        }


        [Fact(DisplayName = nameof(UpdateCategory))]
        [Trait("EndToEnd/API", "Category - Endpoints")]

        public async void UpdateCategory()
        {
            var category = Faker();
            await Persistence.Create(category);

            var request = Faker();
            request.Id = category.Id;

            var (response, outPut) = await apiClient.Put<CustomResponse<CategoryModelOutPut>>(
                $"{Configuration.URL_API_CATEGORY}{category.Id}", 
                request);

            var categoryDb = await Persistence.GetById(category.Id);

            Assert.NotNull(categoryDb);
            Assert.NotNull(response);
            Assert.NotNull(outPut);
            Assert.True(outPut.Success);
            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(outPut.Data.Id, categoryDb.Id);
            Assert.Equal(outPut.Data.Name, categoryDb.Name);   
            Assert.Equal(outPut.Data.IsActive, categoryDb.IsActive);   

        }


        [Fact(DisplayName = nameof(DeleteCategory))]
        [Trait("EndToEnd/API", "Category - Endpoints")]

        public async void DeleteCategory()
        {
            var category = Faker();
            await Persistence.Create(category);

            var request = Faker();
            request.Id = category.Id;

            var (response, outPut) = await apiClient.Delete<CustomResponse<CategoryModelOutPut>>($"{Configuration.URL_API_CATEGORY}{request.Id}");

            var categoryDb = await Persistence.GetById(category.Id);  

            Assert.Null(categoryDb);
            Assert.NotNull(response);
            Assert.NotNull(outPut); 
            Assert.True(outPut.Success);    
            Assert.Equal(System.Net.HttpStatusCode.OK,response.StatusCode);
        }


        [Fact(DisplayName = nameof(GetCategoryById))]
        [Trait("EndToEnd/API", "Category - Endpoints")]

        public async void GetCategoryById()
        {
            var request = Faker();
            await Persistence.Create(request);

            var (response, outPut) = await apiClient.Get<CustomResponse<CategoryModelOutPut>>($"{Configuration.URL_API_CATEGORY}{request.Id}");

            var categoryDb = await Persistence.GetById(request.Id);

            Assert.NotNull(categoryDb);
            Assert.NotNull(response);
            Assert.NotNull(outPut);
            Assert.True(outPut.Success);
            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(outPut.Data.Name, categoryDb.Name);
            Assert.Equal(outPut.Data.IsActive, categoryDb.IsActive);

        }

        [Fact(DisplayName = nameof(ListCategory))]
        [Trait("EndToEnd/API", "Category - Endpoints")]

        public async void ListCategory()
        {
            var qtdCategory = 20;
            var perPager = 10;
            var page = 1;
            var request = GetCategory(qtdCategory);

            await Persistence.CreateList(request);

            var query = new ListCategoryInPut(page, perPager, "", "", SearchOrder.Desc);
            var (response, outPut) = await apiClient.Get<CustomResponse<ListCategoryOutPut>>($"{Configuration.URL_API_CATEGORY}list-category", query);

            var categoryDb = await Persistence.List();

            Assert.Equal(20,categoryDb.Count);
            Assert.NotNull(request); 
            Assert.NotNull(response);
            Assert.NotNull(outPut);
            Assert.True(outPut.Success);    
            Assert.Equal(System.Net.HttpStatusCode.OK,response.StatusCode);
            Assert.NotNull(outPut.Data.Itens);
            Assert.Equal(perPager, outPut.Data.PerPage);
            Assert.Equal(page,outPut.Data.Page);
            Assert.Equal(qtdCategory, outPut.Data.Total);

            foreach(var item in outPut.Data.Itens) 
            {
                var category = await Persistence.GetById(item.Id);
                Assert.NotNull(category);
                Assert.Equal(category.Name, item.Name);
                Assert.Equal(category.IsActive, item.IsActive);
            }

        }

        public void Dispose()
        {
            //throw new NotImplementedException();
        }
    }
}
