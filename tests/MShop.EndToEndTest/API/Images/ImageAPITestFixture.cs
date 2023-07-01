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
using MShop.EndToEndTest.API.Product;
using BusinessEntity = MShop.Business.Entity;
using MShop.EndToEndTest.API.Categoria;

namespace MShop.EndToEndTest.API.Images
{
    public class ImageAPITestFixture :  BaseFixture
    {
        private readonly Guid _productId;
        protected readonly ImagePersistence _imagePersistence;
        protected readonly ProductPersistence _productPersistence;
        protected readonly CategoryPersistence _categoryPersistence;
        public ImageAPITestFixture() 
        { 
            _productId = Guid.NewGuid();
            _imagePersistence = new ImagePersistence(CreateDBContext());
            _productPersistence = new ProductPersistence(CreateDBContext());  
            _categoryPersistence = new CategoryPersistence(CreateDBContext());
        }
        public async Task<CreateImageInPut> FakeRequest(int quantidade = 3)
        {
            var product = await PersistirProduct();
            return new CreateImageInPut { Images = ListFakeImage64(quantidade), ProductId = product.Id };
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

        public BusinessEntity.Image FakeImage()
        {
            return new BusinessEntity.Image(faker.Image.LoremFlickrUrl(), _productId); ;
        }

        public List<BusinessEntity.Image> ListImage(int quantidade = 4)
        {
            var images = new List<BusinessEntity.Image>();
            for(var i = 0; i < quantidade; i++) 
                images.Add(FakeImage());
            return images;
        }

        public async Task<BusinessEntity.Product> PersistirProduct()
        {
            var category = new BusinessEntity.Category(faker.Commerce.Categories(1)[0]);
            await _categoryPersistence.Create(category);  
            var product = new BusinessEntity.Product(faker.Commerce.ProductDescription(),faker.Commerce.ProductName(),10,category.Id,0,true);
            _productPersistence.Create(product);
            return product;

        }

    }
}
