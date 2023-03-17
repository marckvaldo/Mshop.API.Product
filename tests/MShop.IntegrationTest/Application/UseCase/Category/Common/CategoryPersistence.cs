using Microsoft.EntityFrameworkCore;
using BusinessEntity = MShop.Business.Entity;
using MShop.Repository.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MShop.IntegrationTests.Application.UseCase.Category;

public class CategoryPersistence
{
    private readonly RepositoryDbContext _context;

    public CategoryPersistence(RepositoryDbContext context)
    {
        _context = context;
    }

    public async Task<BusinessEntity.Category> GetCategory(Guid id)
    {
        return await _context.Categories.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<IEnumerable<BusinessEntity.Category>> GetAllCategory()
    {
        return await _context.Categories.ToListAsync();
    }

    public async Task Create(BusinessEntity.Category category)
    {
        await _context.Categories.AddAsync(category);
        await _context.SaveChangesAsync();
    }

    public async Task CreateList(IEnumerable<BusinessEntity.Category> categories)
    {
        await _context.AddRangeAsync(categories);
        await _context.SaveChangesAsync();
    }
}
