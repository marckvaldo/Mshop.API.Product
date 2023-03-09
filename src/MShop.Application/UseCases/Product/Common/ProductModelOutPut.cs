using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MShop.Application.UseCases.Product.Common
{
    public class ProductModelOutPut
    {
        public ProductModelOutPut(Guid id, string description, string name, decimal price, string? thumb, decimal stock, bool isActive, Guid categoryId)
        {
            Description = description;
            Name = name;
            Price = price;
            Stock = stock;
            IsActive = isActive;
            CategoryId = categoryId;
            Id = id;
            Thumb = thumb;  
        }
        public Guid Id { get; set; }

        public string Description { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public decimal Stock { get;  set; }

        public bool IsActive { get; set; }

        public Guid CategoryId { get; set; }

        public string? Thumb { get; set; }
    }
}
