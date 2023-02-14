﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MShop.Business.Interface.Cache
{
    public interface ICacheRepository
    {
        Task<TResult?> GetKey<TResult>(string key);
        Task SetKey(string key, object value, TimeSpan TimeExpiration);
        Task SetKeyCollection(string key, object value, TimeSpan TimeExpiration);
        Task DeleteKey(string key);
        Task<List<TResult?>?> GetKeyCollection<TResult>(string key);
    }
}
