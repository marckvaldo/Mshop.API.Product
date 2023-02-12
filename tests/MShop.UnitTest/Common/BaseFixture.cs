using Bogus;
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
    }
}
