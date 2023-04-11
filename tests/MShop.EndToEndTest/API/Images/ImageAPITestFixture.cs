using Microsoft.AspNetCore.Builder;
using MShop.Application.Common;
using MShop.Application.UseCases.images.CreateImage;
using MShop.EndToEndTest.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using MShop.Business.Entity;

namespace MShop.EndToEndTest.API.Images
{
    public class ImageAPITestFixture :  BaseFixture
    {
        private readonly Guid _productId;
        protected readonly ImagePersistence _imagePersistence;    
        public ImageAPITestFixture() 
        { 
            _productId = Guid.NewGuid();
            _imagePersistence = new ImagePersistence(CreateDBContext());
        }
        public CreateImageInPut FakeRequest(int quantidade = 3)
        {
            return new CreateImageInPut { Images = ListFakeImage64(quantidade), ProductId = _productId };
        }

        public FileInputBase64 FakeImage64()
        {
            return new FileInputBase64(FileFakerBase64.IMAGE64);
        }

        public List<FileInputBase64> ListFakeImage64(int quantidade = 3)
        {
            var images = new List<FileInputBase64>();
            for(int i = 0;i < quantidade; i++)
                images.Add(FakeImage64());

            return images;
        }

        public Image FakeImage()
        {
            return new Image(faker.Image.LoremFlickrUrl(), _productId); ;
        }

        public List<Image> ListImage(int quantidade = 4)
        {
            var images = new List<Image>();
            for(var i = 0; i < quantidade; i++) 
                images.Add(FakeImage());
            return images;
        }

    }
}
