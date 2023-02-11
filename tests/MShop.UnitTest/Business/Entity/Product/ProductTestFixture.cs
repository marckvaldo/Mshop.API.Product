using Mshop.Test.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using BusinessEntity = MShop.Business.Entity;

namespace Mshop.Test.Business.Entity.Product
{
    public abstract class ProductTestFixture : BaseFixture
    {
        private readonly Guid _categoryId;

        protected ProductTestFixture()
        {
            _categoryId = Guid.NewGuid();
        }

        protected BusinessEntity.Product GetProductValid()
        {
            return new(Fake().Description, Fake().Name, Fake().Price, Fake().Imagem,Fake().CategoryId,Fake().Stock,Fake().IsActive);
        }
        protected BusinessEntity.Product GetProductValid(ProductFake fake)
        {
            return new(fake.Description, fake.Name, fake.Price, fake.Imagem, fake.CategoryId, fake.Stock, fake.IsActive);
        }

        protected ProductFake Fake()
        {
            return new ProductFake
            {
                Name = faker.Commerce.ProductName(),
                Description = faker.Commerce.ProductDescription(),
                Price = Convert.ToDecimal(faker.Commerce.Price()),
                Imagem = faker.Image.LoremPixelUrl(),
                CategoryId = _categoryId,
                Stock = faker.Random.UInt(),
                IsActive = true
            };
        }

        protected ProductFake Fake(string description, string name, decimal price, string imagem, Guid categoryId, decimal stock, bool isActive = true)
        {
            return new ProductFake
            {
                Description = description,
                Name = name,
                Price = price,
                Imagem = imagem,
                CategoryId = categoryId,
                Stock = stock,
                IsActive = isActive
            };
        }


        //Description Invalid
        public static IEnumerable<object[]> GetDescriptionProductInvalid()
        {
            yield return new object[] { GetDescriptionProductGreaterThan1000CharactersInvalid() };
            yield return new object[] { GetDescriptionProductLessThan10CharactersInvalid() };
            yield return new object[] { "" };
            yield return new object[] { " " };
            yield return new object[] { null };
        }
       
        private static string GetDescriptionProductGreaterThan1000CharactersInvalid()
        {
            string description = fakerStatic.Commerce.ProductDescription();
            while (description.Length < 1001)
            {
                description += fakerStatic.Commerce.ProductDescription();
            }

            return description;
        }

        private static string GetDescriptionProductLessThan10CharactersInvalid()
        {
            string category = fakerStatic.Commerce.ProductDescription();
            category = category[..9];
            return category;
        }


        //Name Invalid
        public static IEnumerable<object[]> GetNameProductInvalid()
        {
            yield return new object[] { GetNameProductGreaterThan255CharactersInvalid() };
            yield return new object[] { GetNameProductLessThan3CharactersInvalid() };
            yield return new object[] { "" };
            yield return new object[] { " " };
            yield return new object[] { null };
        }
        private static string GetNameProductGreaterThan255CharactersInvalid()
        {
            string description = fakerStatic.Commerce.ProductName();
            while (description.Length < 256)
            {
                description += fakerStatic.Commerce.ProductName();
            }

            return description;
        }
        private static string GetNameProductLessThan3CharactersInvalid()
        {
            string product = fakerStatic.Commerce.ProductName();
            product = product[..2];
            return product;
        }
    }

    public class ProductFake
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string Imagem { get; set; }
        public Guid CategoryId { get; set; }
        public decimal Stock { get; set; }
        public bool IsActive { get; set; }
    }
}
