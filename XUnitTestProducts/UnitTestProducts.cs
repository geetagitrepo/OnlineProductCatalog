using Microsoft.EntityFrameworkCore;
using Moq;
using System.Collections.Generic;
using System.Linq;
using ProductCatalog.Models;
using ProductCatalog.EF;
using ProductCatalog.Repository;
using Xunit;

namespace Products.Tests
{
    public class productRepositoryTests
    {
        [Fact]
        public async void SaveProductTest()
        {
            IQueryable<Product> products = new List<Product>
            {
                new Product
                {
                    Name = "Product 1",
                    Price = 100m,
                    Code = "P01"
                },
                new Product
                {
                    Name = "Product 2",
                    Price = 200m,
                    Code = "P02"
                }

            }.AsQueryable();

            var mockSet = new Mock<DbSet<Product>>();

            mockSet.As<IAsyncEnumerable<Product>>()
           .Setup(d => d.GetEnumerator())
           .Returns(new AsyncEnumerator<Product>(products.GetEnumerator()));

            mockSet.As<IQueryable<Product>>().Setup(m => m.Provider).Returns(products.Provider);
            mockSet.As<IQueryable<Product>>().Setup(m => m.Expression).Returns(products.Expression);
            mockSet.As<IQueryable<Product>>().Setup(m => m.ElementType).Returns(products.ElementType);
            mockSet.As<IQueryable<Product>>().Setup(m => m.GetEnumerator()).Returns(products.GetEnumerator());

            var mockContext = new Mock<ProductContext>();
            mockContext.Setup(c => c.Products).Returns(mockSet.Object);

            var repository = new ProductRepository(mockContext.Object);
            var actual = await repository.AddAsync(products.FirstOrDefault());

            Assert.Equal("P01", actual.Code);
            Assert.Equal("Product 1", actual.Name);
            Assert.Equal(100m, actual.Price);
        }

        [Fact]
        public async void UpdateProductTest()
        {
            IQueryable<Product> products = new List<Product>
            {
                new Product
                {
                    Name = "Product 1",
                    Price = 100m,
                    Code = "P01"
                },
                new Product
                {
                    Name = "Product 2",
                    Price = 200m,
                    Code = "P02"
                }

            }.AsQueryable();

            var mockSet = new Mock<DbSet<Product>>();

            mockSet.As<IAsyncEnumerable<Product>>()
           .Setup(d => d.GetEnumerator())
           .Returns(new AsyncEnumerator<Product>(products.GetEnumerator()));

            mockSet.As<IQueryable<Product>>().Setup(m => m.Provider).Returns(products.Provider);
            mockSet.As<IQueryable<Product>>().Setup(m => m.Expression).Returns(products.Expression);
            mockSet.As<IQueryable<Product>>().Setup(m => m.ElementType).Returns(products.ElementType);
            mockSet.As<IQueryable<Product>>().Setup(m => m.GetEnumerator()).Returns(products.GetEnumerator());

            var mockContext = new Mock<ProductContext>();
            mockContext.Setup(c => c.Products).Returns(mockSet.Object);

            var repository = new ProductRepository(mockContext.Object);

            int resposne = await repository.UpdateAsync(products.FirstOrDefault());

            Assert.Equal(0, resposne);
        }

        [Fact]
        public async void FindProductTest()
        {
            IQueryable<Product> products = new List<Product>
            {
                new Product
                {
                    Id=1,
                    Name = "Product 1",
                    Price = 100m,
                    Code = "P01"
                },
                new Product
                {
                    Id=2,
                    Name = "Product 2",
                    Price = 200m,
                    Code = "P02"
                }

            }.AsQueryable();

            var mockSet = new Mock<DbSet<Product>>();
            mockSet.As<IQueryable<Product>>()
                      .Setup(d => d.Provider)
                      .Returns(new TestAsyncQueryProvider<Product>(products.Provider));

            mockSet.As<IQueryable<Product>>().Setup(m => m.Expression).Returns(products.Expression);
            mockSet.As<IQueryable<Product>>().Setup(m => m.ElementType).Returns(products.ElementType);
            mockSet.As<IQueryable<Product>>().Setup(m => m.GetEnumerator()).Returns(products.GetEnumerator());

            var mockContext = new Mock<ProductContext>();
            mockContext.Setup(m => m.Products).Returns(mockSet.Object);

            var repository = new ProductRepository(mockContext.Object);
            var actual = await repository.FindAsync((int)products.FirstOrDefault().Id);

            Assert.Equal("P01", actual.Code);
            Assert.Equal("Product 1", actual.Name);
            Assert.Equal(100m, actual.Price);
        }        
    }
}