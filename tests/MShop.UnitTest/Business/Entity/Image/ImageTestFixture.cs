using Bogus;
using Mshop.Test.Common;
using BusinessEntity = MShop.Business.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.StackExchangeRedis;

namespace MShop.UnitTests.Business.Entity.Image
{
    public class ImageTestFixture : BaseFixture
    {
        private readonly Guid _id;
        public ImageTestFixture()
        {
            _id = Guid.NewGuid();
        }

        public BusinessEntity.Image Faker()
        {
            return new BusinessEntity.Image(faker.Image.LoremPixelUrl(), _id);
        }

        public BusinessEntity.Image Faker(Guid id, string FileName)
        {
            return new BusinessEntity.Image(FileName, id);
        }


        public IReadOnlyList<BusinessEntity.Image> FakerImages(Guid id, int quantity)
        {
            List<BusinessEntity.Image> images = new List<BusinessEntity.Image>();
            for(int i = 0; i < quantity; i++)
                images.Add(new BusinessEntity.Image(Faker().FileName, _id));

            return images;
        }
    }
}
