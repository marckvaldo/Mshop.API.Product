using Bogus;
using MShop.Application.Common;
using MShop.UnitTests.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mshop.Test.Common
{
    public abstract class BaseFixture
    {
        protected readonly Faker faker;
        public static readonly Faker fakerStatic = new Faker("pt_BR");
        protected BaseFixture()
        {
            faker = new Faker("pt_BR"); 
        }

        protected static FileInputBase64 ImageFake64()
        {
            return new FileInputBase64(FileFakerBase64.IMAGE64);
        }

        protected static string ExtensionFile(string file)
        {
            return Helpers.GetExtensionBase64(file);
        }

    }
}
