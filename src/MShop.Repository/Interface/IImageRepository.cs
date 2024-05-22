using MShop.Business.Entity;
using MShop.Core.Data;


namespace MShop.Repository.Interface
{
    public interface IImageRepository : IRepository<Image>
    {
        Task CreateRange(List<Image> images, CancellationToken cancellationToken);

        Task DeleteByIdProduct(Guid productId);
    }
}
