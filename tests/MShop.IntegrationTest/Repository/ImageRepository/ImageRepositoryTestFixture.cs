using Bogus;
using MShop.Business.Entity;
using MShop.IntegrationTests.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MShop.IntegrationTests.Repository.ImagenRepository
{
    public class ImageRepositoryTestFixture : BaseFixture
    {
        private Guid ProductId;

        public ImageRepositoryTestFixture()
        {
            ProductId = Guid.NewGuid();
        }

        public Image Faker()
        {

            return new Image(faker.Image.LoremFlickrUrl(), ProductId);  
        }

        public Image Faker(Guid productId)
        {
            return new Image(faker.Image.LoremFlickrUrl(), productId);
        }
    
        public List<Image> FakerImages(int quantity)
        {
            List<Image> FakerList = new ();
            for (int i = 0; i < quantity; i++)
                FakerList.Add(Faker());

            return FakerList;
        }


        public List<Image> FakerImages(int quantity, Guid productId)
        {
            List<Image> FakerList = new();
            for (int i = 0; i < quantity; i++)
                FakerList.Add(Faker(productId));

            return FakerList;
        }
    }
}

