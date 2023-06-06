using MShop.Business.Entity;

namespace MShop.Business.Interface.Service
{
    public interface IProductService
    {
        Task Add(Product produtc);

        Task Update(Product produtc);

        Task Remover(Guid id);
    }
}
