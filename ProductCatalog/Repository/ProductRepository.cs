using ProductCatalog.EF;
using ProductCatalog.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace ProductCatalog.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly ProductContext context;

        public ProductRepository(ProductContext context)
        {
            this.context = context;
        }

        public async Task<Product> AddAsync(Product item)
        {
            context.Add(item);
            await context.SaveChangesAsync();

            return item;
        }

        public IEnumerable<Product> GetAll()
        {
            return context.Products.OrderByDescending(p => p.Id);
        }

        public IEnumerable<Product> GetAll(int pageNumber, int pageSize)
        {
            return context.Products.OrderBy(e => e.Id).Skip((pageNumber-1) * pageSize).Take(pageSize).OrderByDescending(p => p.Id); 
        }

        public async Task<Product> FindAsync(int id)
        {
            return await context.Products.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task RemoveAsync(int id)
        {
            var entity = await context.Products.SingleOrDefaultAsync(p => p.Id == id);
            if (entity == null)
                throw new InvalidOperationException("No product found matching id " + id);

            context.Products.Remove(entity);

            await context.SaveChangesAsync();
        }

        public async Task<int> UpdateAsync(Product item)
        {
            int response = 0;
            if (item == null)
                throw new ArgumentNullException(nameof(item));
            try
            {
                context.Products.Update(item);
                response = await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                ex.Entries[0].Reload();
                context.SaveChanges();
            }
            return response;
        }
    }
}
