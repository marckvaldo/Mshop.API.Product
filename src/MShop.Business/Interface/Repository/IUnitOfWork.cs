using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MShop.Business.Interface.Repository
{
    public interface IUnitOfWork
    {
        public Task CommitAsync(CancellationToken cancellationToken = default);

        public Task RollbackAsync(CancellationToken cancellationToken = default);
    }
}
