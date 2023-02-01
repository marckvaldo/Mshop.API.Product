using Mshop.Test.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessEntity = MShop.Business.Entity;

namespace Mshop.Tests.Application.UseCases.Product.ListProducts
{
    public class ListProductTestFixture : BaseFixture
    {
        private readonly Guid _categoryId;
        private readonly Guid _id;
        public List<string> fakeContantsNames;
        public ListProductTestFixture() : base()
        {
            _categoryId = Guid.NewGuid();
            _id = Guid.NewGuid();

            fakeContantsNames = new()
            {
                "ASP",
                "C#",
                "DARCK",
                "PHP"
            };
        }

        protected BusinessEntity.Product Faker()
        {
            var product = (new BusinessEntity.Product
            (
                faker.Commerce.ProductName(),
                faker.Commerce.ProductDescription(),
                Convert.ToDecimal(faker.Commerce.Price()),
                faker.Image.LoremPixelUrl(),
                _categoryId,
                faker.Random.UInt(),
                true
            ));
            product.Id = _id;
            return product;
        }

        protected List<BusinessEntity.Product> GetListProduts(int length = 10)
        {
            var products = new List<BusinessEntity.Product>();
            for(int i = 0; i < length; i++)
                products.Add(Faker());
            return products;
        }

        protected IReadOnlyList<BusinessEntity.Product> GetListProdutsConstant()
        {
            var products = GetListProduts(4);

            int i = 0;
            foreach (var item in products)
            {
                item.Update(item.Description, fakeContantsNames[i], item.Price, item.CategoryId);
                i++;
            }
            
            return products;    
        }
    }
}
