using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MShop.Business.Interface.Cache
{
    public interface ICacheRepository
    {
        Task<TResult?> GetKey<TResult>(string key);
        Task SetKey<TResult>(string key, TResult value);
        Task SetKeyCollection<TResult>(string key, List<TResult> value);
        Task DeleteKey(string key);
        Task<List<TResult?>?> GetKeyCollection<TResult>(string key);
    }
}
