using ProductCatalog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductCatalog.Repository
{
    public interface IProductRepository
    {
        Task<Product> AddAsync(Product product);
        IEnumerable<Product> GetAll();
        Task<Product> FindAsync(int productId);
        Task RemoveAsync(int productId);
        Task<int> UpdateAsync(Product product);
        IEnumerable<Product> GetAll(int pageNumber, int pageSize);

    }
}
