using FHSWebServiceApplication.Data;
using FHSWebServiceApplication.Interfaces;
using FHSWebServiceApplication.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System.Data;

namespace FHSWebServiceApplication.Repositories
{
    public class ProductRepository : IRepository<Product>
    {
        private readonly AppDbContext _context;
        private readonly IMemoryCache _cache;

        public ProductRepository(AppDbContext context, IMemoryCache cache)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
        }

        public Product Create(Product product)
        {
            _context.Products.Add(product);
            _context.SaveChanges();

            _cache.Remove("AllProducts");

            return product;
        }

        public Product GetById(int id)
        {
            if (id <= 0)
            {
                throw new Exception("Some thing went wrong.");
            }

            var product = _cache.GetOrCreate(
                $"Product_{id}", context =>
                {
                    context.SetAbsoluteExpiration(TimeSpan.FromMinutes(10));
                    context.SetPriority(CacheItemPriority.High);

                    return _context.Products.AsNoTracking().FirstOrDefault(p => p.Id == id);
                });

            return product;
        }

        public IEnumerable<Product> GetAll()
        {
            var product = _cache.GetOrCreate(
                $"AllProducts", context =>
                {
                    context.SetAbsoluteExpiration(TimeSpan.FromMinutes(10));
                    context.SetPriority(CacheItemPriority.High);

                    return _context.Products.AsNoTracking().ToList();
                });

            return product;
        }

        public Product Update(Product product)
        {
            _context.Products.Update(product);
            _context.SaveChanges();

            _cache.Remove($"Product_{product.Id}");
            _cache.Remove("AllProducts");

            return product;
        }

        public void Delete(int id)
        {
            var product = _context.Products.FirstOrDefault(p => p.Id == id);
            if (product != null)
            {
                _context.Products.Remove(product);
                _context.SaveChanges();

                _cache.Remove($"Product_{id}");
                _cache.Remove("AllProducts");
            }
        }
    }
}
