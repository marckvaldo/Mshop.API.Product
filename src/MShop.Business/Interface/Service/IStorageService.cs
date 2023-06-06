namespace MShop.Business.Interface.Service
{
    public interface IStorageService
    {
        Task<string> Upload(string FileName, Stream FileStreang);

        Task<bool> Delete(string FileName);
    }
}
