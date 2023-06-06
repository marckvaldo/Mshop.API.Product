using MShop.Business.Entity;

namespace MShop.Business.Interface.Repository
{
    public interface IImageRepository : IRepository<Image>
    {
        Task CreateRange(List<Image> images, CancellationToken cancellationToken);

        Task DeleteByIdProduct(Guid productId);
    }
}
