using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories
{
  internal class ProductRepositoryAsync: GenericRepositoryAsync<Product>, IProductRepositoryAsync
  {
    private readonly DbSet<Product> _products;
    private readonly ApplicationDbContext _context;

    public ProductRepositoryAsync(ApplicationDbContext dbContext) : base(dbContext)
    {
      _products = dbContext.Products;
      _context = dbContext;
    }

    public async Task<IReadOnlyList<Product>> GetPagedReponseWithRelationsAsync(int pageNumber, int pageSize)
    {
      return await _products
          .Skip((pageNumber - 1) * pageSize)
          .Take(pageSize)
          .Include(p => p.Images)
          .Include(p => p.Category)
          .Include(p => p.Seller)
          .ThenInclude(s => s.Address)
          .AsNoTracking()
          .ToListAsync();
    }

    public async Task<Product?> GetByIdWithRelationsAsync(int id)
    {
      return await _products
        .Include(p => p.Images)
        .Include(p => p.Category)
        .Include(p => p.Seller)
        .ThenInclude(s => s.Address)
        .AsNoTracking()
        .SingleOrDefaultAsync(p => p.Id == id);
    }

    public async Task<IReadOnlyList<Product>> GetByCategoryIdWithRelationsAsync(int id, int pageNumber, int pageSize)
    {
      return await _products
        .Include(p => p.Images)
        .Include(p => p.Category)
        .Include(p => p.Seller)
        .ThenInclude(s => s.Address)
        .Where(p => p.Category.Id == id)
        .Skip((pageNumber - 1) * pageSize)
        .Take(pageSize)
        .AsNoTracking()
        .ToListAsync();
    }

    public async Task<IReadOnlyList<Product>> GetBySellerIdentityIdWithRelationsAsync(string id, int pageNumber, int pageSize)
    {
      return await _products
        .Include(p => p.Images)
        .Include(p => p.Category)
        .Include(p => p.Seller)
        .ThenInclude(s => s.Address)
        .Where(p => p.Seller.IdentityId == id)
        .Skip((pageNumber - 1) * pageSize)
        .Take(pageSize)
        .AsNoTracking()
        .ToListAsync();
    }

    public async Task DeleteImageAsync(Image image)
    {
      _context.Set<Image>().Remove(image);
      await _context.SaveChangesAsync();
    }

    public async Task<Image> AddImageAsync(Image image)
    {
      await _context.Set<Image>().AddAsync(image);
      await _context.SaveChangesAsync();
      return image;
    }

    public async Task<Image?> GetImageByIdAsync(int id)
    {
      return await _context.Set<Image>()
        .AsNoTracking()
        .SingleOrDefaultAsync(x => x.Id == id);
    }

    public async Task<Image?> GetImageByUrlAsync(string url)
    {
      return await _context.Set<Image>()
        .AsNoTracking()
        .SingleOrDefaultAsync(x => x.Url == url);
    }

    public async Task UpdateImageAsync(Image image)
    {
      _context.Entry(image).State = EntityState.Modified;
      await _context.SaveChangesAsync();
    }

    public async Task<int> GetDataCountByCategoryIdAsync(int id)
    {
      return await _products
        .Include(p => p.Category)
        .Where(p => p.Category.Id == id)
        .CountAsync();
    }

    public async Task<int> GetDataCountBySellerIdentityIdAsync(string id)
    {
      return await _products
        .Include(p => p.Seller)
        .Where(p => p.Seller.IdentityId == id)
        .CountAsync();
    }
  }
}
