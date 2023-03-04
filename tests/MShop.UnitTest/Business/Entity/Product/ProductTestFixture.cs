using Mshop.Test.Common;
using MShop.Business.ValueObject;
using MShop.UnitTests.Common;
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
            return new(Fake().Description, Fake().Name, Fake().Price, Fake().CategoryId,Fake().Stock,Fake().IsActive);
        }
        protected BusinessEntity.Product GetProductValid(ProductFake fake)
        {
            return new(fake.Description, fake.Name, fake.Price, fake.CategoryId, fake.Stock, fake.IsActive);
        }

        protected ProductFake Fake()
        {
            return new ProductFake
            {
                Name = faker.Commerce.ProductName(),
                Description = faker.Commerce.ProductDescription(),
                Price = Convert.ToDecimal(faker.Commerce.Price()),
                Imagem = new Image(faker.Image.LoremPixelUrl()),
                CategoryId = _categoryId,
                Stock = faker.Random.UInt(),
                IsActive = true
            };
        }

        protected ProductFake Fake(string description, string name, decimal price, Image imagem, Guid categoryId, decimal stock, bool isActive = true)
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


        public static IEnumerable<object[]> ListNameProductInvalid()
        {
            yield return new object[] { InvalidData.GetNameProductGreaterThan255CharactersInvalid() };
            yield return new object[] { InvalidData.GetNameProductLessThan3CharactersInvalid() };
            yield return new object[] { "" };
            yield return new object[] { " " };
            yield return new object[] { null };
        }

        public static IEnumerable<object[]> ListDescriptionProductInvalid()
        {
            yield return new object[] { InvalidData.GetDescriptionProductGreaterThan1000CharactersInvalid() };
            yield return new object[] { InvalidData.GetDescriptionProductLessThan10CharactersInvalid() };
            yield return new object[] { "" };
            yield return new object[] { " " };
            yield return new object[] { null };
        }


    }

    public class ProductFake
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public Image Imagem { get; set; }
        public Guid CategoryId { get; set; }
        public decimal Stock { get; set; }
        public bool IsActive { get; set; }
    }
}
