﻿using MShop.Application.UseCases.Category.Common;
using MShop.Application.UseCases.Product.Common;
using MShop.Business.ValueObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MShop.Application.UseCases.Product.GetProduct
{
    public class GetProductOutPut : ProductModelOutPut
    {
        public GetProductOutPut(Guid id, 
            string description, 
            string name, 
            decimal price, 
            string? thumb, 
            decimal stock, 
            bool isActive, 
            Guid categoryId,
            CategoryModelOutPut category,
            List<string?> images,
            bool isPromotion)
            : base(id, description,name,price,thumb,stock,isActive,categoryId,category,isPromotion)
        {
            Description = description;
            Name = name;
            Price = price;
            Stock = stock;
            IsActive = isActive;
            CategoryId = categoryId;
            Id = id;
            Thumb = thumb;
            Images = images;
        }
       
        public List<string?> Images { get; set; }   
    }
}
