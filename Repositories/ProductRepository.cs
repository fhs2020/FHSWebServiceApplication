using FHSWebServiceApplication.Data;
using FHSWebServiceApplication.Interfaces;
using FHSWebServiceApplication.Model;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FHSWebServiceApplication.Repositories
{
  public class ProductRepository : IRepository<Product>
  {
    private readonly AppDbContext _context;
    private readonly IMemoryCache _cache;

    public ProductRepository(AppDbContext context, IMemoryCache cache)
    {
      _context = context;
      _cache = cache;
    }

    public Product Create(Product entity)
    {
      _context.Products.Add(entity);
      _context.SaveChanges();

      // Clear the cache since the data has changed
      _cache.Remove("AllProducts");

      return entity;
    }

    public Product GetById(int id)
    {
      // Check if the data is in the cache
      if (_cache.TryGetValue($"Product_{id}", out Product cachedProduct))
      {
        return cachedProduct;
      }

      // If not in cache, fetch from the database
      var product = _context.Products.FirstOrDefault(p => p.Id == id);

      // Store in cache for future use
      if (product != null)
      {
        var cacheEntryOptions = new MemoryCacheEntryOptions
        {
          AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10) // Cache for 10 minutes 
        };

        _cache.Set($"Product_{id}", product, cacheEntryOptions);
      }

      return product;
    }

    public IEnumerable<Product> GetAll()
    {
      // Check if the data is in the cache
      if (_cache.TryGetValue("AllProducts", out IEnumerable<Product> cachedProducts))
      {
        return cachedProducts;
      }

      // If not in cache, fetch from the database
      var products = _context.Products.ToList();

      // Store in cache for future use
      if (products != null && products.Any())
      {
        var cacheEntryOptions = new MemoryCacheEntryOptions
        {
          AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10) // Cache for 10 minutes
        };

        _cache.Set("AllProducts", products, cacheEntryOptions);
      }

      return products;
    }

    public Product Update(Product entity)
    {
      _context.Products.Update(entity);
      _context.SaveChanges();

      // Clear the cache  since the data has changed
      _cache.Remove($"Product_{entity.Id}");
      _cache.Remove("AllProducts");

      return entity;
    }

    public void Delete(int id)
    {
      var product = _context.Products.FirstOrDefault(p => p.Id == id);
      if (product != null)
      {
        _context.Products.Remove(product);
        _context.SaveChanges();

        // Clear the cache (if applicable) since the data has changed
        _cache.Remove($"Product_{id}");
        _cache.Remove("AllProducts");
      }
    }
  }
}
