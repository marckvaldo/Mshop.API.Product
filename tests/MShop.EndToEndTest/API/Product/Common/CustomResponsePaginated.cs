namespace MShop.EndToEndTest.API.Product.Common
{
    public class CustomResponsePaginated<TResult>
    {

        public Paginated<TResult> Data { get; set; }

        public bool Success { get; set; }

        public CustomResponsePaginated(Paginated<TResult> data, bool success)
        {
            Data = data;
            Success = success;
        }

        
    }
}
