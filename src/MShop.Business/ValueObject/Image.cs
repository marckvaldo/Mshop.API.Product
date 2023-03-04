using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MShop.Business.ValueObject
{
    public class Image
    {
        public string Path { get; private set; }
        public Image(string? path)
        {
            Path = path;
        }
    }
}
