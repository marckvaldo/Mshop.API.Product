

using MShop.Business.Entity;
using MShop.Core.Exception;
using MShop.Core.Message;

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

            //Action action = () => image.IsValid(_notifications);
            //var exception = Assert.Throws<EntityValidationException>(action);   

            Assert.False(image.IsValid(_notifications));
            Assert.True(_notifications.HasErrors());
        }
    }
}
