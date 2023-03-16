using Microsoft.EntityFrameworkCore;
using BusinessEntity = MShop.Business.Entity;
using MShop.Repository.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MShop.IntegrationTests.Application.UseCase.Product;

public class ProductPersistence
{
    private readonly RepositoryDbContext _context;

    public ProductPersistence(RepositoryDbContext context)
    {
        _context = context;
    }

    public async Task<BusinessEntity.Product> GetProduct(Guid id)
    {
        return await _context.Products.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<IEnumerable<BusinessEntity.Product>> GetAllProduct()
    {
        return await _context.Products.ToListAsync();
    }

    public async Task Create(BusinessEntity.Product product)
    {
        await _context.Products.AddAsync(product);
        await _context.SaveChangesAsync();
    }

    public async Task CreateList(IEnumerable<BusinessEntity.Product> products)
    {
        await _context.Products.AddRangeAsync(products);
        await _context.SaveChangesAsync();
    }
}
