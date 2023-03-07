using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MShop.Business.Interface.Service
{
    public interface IStorageService
    {
        Task<string> Upload(string FileName, Stream FileStreang);

        Task<bool> Delete(string FileName);
    }
}
