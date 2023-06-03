using Bogus;
using MShop.Business.Entity;
using MShop.Business.Exceptions;
using MShop.Business.Interface;
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
        private readonly Notifications _notifications;

        public ImageTest()
        {
            _notifications = new Notifications();
        }

        [Fact(DisplayName = nameof(Instantiate))]
        [Trait("Business", "Image")]

        public void Instantiate()
        {
            var FileName = Faker().FileName;
            var image = Faker(Faker().Id, FileName);
            image.IsValid(_notifications);

            Assert.NotNull(image);
            Assert.NotEqual(image.Id,Guid.Empty);
            Assert.Equal(image.FileName, FileName);            
        }


        [Theory(DisplayName = nameof(ShoudReturErroInstantiate))]
        [Trait("Business", "Image")]
        [InlineData(null,"")]
        [InlineData(null, "image")]
        [InlineData(null,null)]
        public void ShoudReturErroInstantiate(Guid productId, string fileName)
        {            
            var image = Faker(productId, fileName);
           
            Action action =
                 () => image.IsValid(_notifications);

            var exception = Assert.Throws<EntityValidationException>(action);
            Assert.True(_notifications.HasErrors());
        }
    }
}
