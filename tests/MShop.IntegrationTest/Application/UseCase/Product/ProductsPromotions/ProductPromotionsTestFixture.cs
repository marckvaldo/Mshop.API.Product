using MShop.Business.Entity;
using MShop.IntegrationTests.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessEntity = MShop.Business.Entity;

namespace MShop.IntegrationTests.Application.UseCase.Product.ProductsPromotions;

public class ProductPromotionsTestFixture : BaseFixture
{
  
    private readonly Guid _categoryId;
    public ProductPromotionsTestFixture() : base()
    {
        _categoryId = Guid.NewGuid();
    }

    protected BusinessEntity.Product Faker()
    {
        return new BusinessEntity.Product
        (
            faker.Commerce.ProductDescription(),
            faker.Commerce.ProductName(),
            Convert.ToDecimal(faker.Commerce.Price()),
            faker.Image.LoremPixelUrl(),
            _categoryId,
            faker.Random.UInt(),
            true
        );
    }

    protected List<BusinessEntity.Product> FakerList(int length = 5)
    {
        List<BusinessEntity.Product> listProduct = new List<BusinessEntity.Product>();

        for (int i = 0; i < length; i++)
            listProduct.Add(Faker());

        return listProduct;
    }
}
