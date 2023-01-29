using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using BusinessEntity = MShop.Business.Entity;

namespace Mshop.Test.Business.Entity.Product
{
    public abstract class ProductTestFixture
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
                Name = "Product Name",
                Description = "Product Name Product Name",
                Price = 10,
                Imagem = "",
                CategoryId = _categoryId,
                Stock = 2,
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
