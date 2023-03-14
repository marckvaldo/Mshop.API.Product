using MShop.Application.UseCases.images.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MShop.Application.UseCases.images.ListImage
{
    public interface IListImage
    {
        Task<ListImageOutPut> Handler (Guid productId);
    }
}
