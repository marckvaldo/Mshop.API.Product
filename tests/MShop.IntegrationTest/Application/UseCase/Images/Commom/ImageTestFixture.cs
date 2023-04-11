using MShop.Application.Common;
using MShop.Business.Entity;
using MShop.IntegrationTests.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MShop.IntegrationTests.Application.UseCase.Images.Commom
{
    public class ImageTestFixture : BaseFixture
    {
        public Image Faker(Guid productId)
        {
            return new Image(faker.Image.PlaceImgUrl(), productId);
        }

        public List<Image> FakerList(Guid productId, int quantity)
        {
            List<Image> list = new List<Image>();
            for (int i = 0; i < quantity; i++)
                list.Add(Faker(productId));

            return list;
        }

        public FileInput FakeFileInput()
        {
            return new FileInput("jpg", new MemoryStream(Encoding.ASCII.GetBytes(fakerStatic.Image.LoremPixelUrl())));
        }

        public List<FileInput> FakeFileInputList(int quantity = 3)
        {
            List<FileInput> list = new List<FileInput>();
            for(int i = 0; i < quantity;i++)
                list.Add(FakeFileInput());

            return list;
        }

        public List<FileInputBase64> FakeFileInputList64(int quantity = 3)
        {
            List<FileInputBase64> list = new List<FileInputBase64>();
            for (int i = 0; i < quantity; i++)
                list.Add(ImageFake64());

            return list;
        }

    }
}
