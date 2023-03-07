using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MShop.Business.ValueObject
{
    public class FileImage
    {
        public string Path { get; private set; }
        public FileImage(string? path)
        {
            Path = path;
        }
    }
}
