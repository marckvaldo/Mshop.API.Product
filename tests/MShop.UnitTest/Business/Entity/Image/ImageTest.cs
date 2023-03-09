using Bogus;
using MShop.Business.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MShop.UnitTests.Business.Entity.Image
{
    public class ImageTest : ImageTestFixture
    {
        [Fact(DisplayName = nameof(Instantiate))]
        [Trait("Business", "Image")]

        public void Instantiate()
        {
            var notificacao = new Notifications();
            var FileName = Faker().FileName;
            var product = Faker(Faker().Id, Faker().FileName);
            product.IsValid(notificacao);

            Assert.NotNull(product);
            Assert.Equal(product.FileName, FileName);            
        }
    }
}
