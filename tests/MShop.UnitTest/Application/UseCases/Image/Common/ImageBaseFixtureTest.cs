using Mshop.Test.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessEntity = MShop.Business.Entity;
using ApplicationUseCase = MShop.Application.UseCases.images;
using MShop.Application.Common;

namespace MShop.UnitTests.Application.UseCases.Image.Common
{
    public class ImageBaseFixtureTest : BaseFixture
    {
        public BusinessEntity.Image Faker(Guid productId)
        {
            return new BusinessEntity.Image(faker.Image.LoremFlickrUrl(), productId);
        }

        public List<BusinessEntity.Image> FakerImages(Guid productId, int quantity)
        {
            List<BusinessEntity.Image> listImages = new List<BusinessEntity.Image>();
            for (int i = 1; i <= quantity; i++)
                listImages.Add(Faker(productId));
            
            return listImages;
        }


        public FileInput ImageFaker()
        {
            return new FileInput("jpg", new MemoryStream(Encoding.ASCII.GetBytes(fakerStatic.Image.LoremPixelUrl())));
        }


        public List<FileInput> ImageFakers(int quantity)
        {
            List<FileInput> listImage = new List<FileInput>();
            for (int i = 0; i <= quantity; i++)
                listImage.Add(ImageFaker());

            return listImage;
        }
    }
}
