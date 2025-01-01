using Moq;
using Moq.EntityFrameworkCore;
using NUnit.Framework;
using Products.Api.Services;
using Products.Api.Entities;
using Products.Api.DbContexts;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace ProdutApi.Tests
{
    
    internal class ProductRepository_Tests
    {

        private Mock<IProductContext> _context;
        private ProductRepository _dataAccess;
        [SetUp]
        public void SetUp()
        {
            _context = new Mock<IProductContext>();
            var products = new List<Product>() { new Product() { Id = 1, ProductName = "Name" } };
            _context.Setup(x => x.Products).ReturnsDbSet(products).Verifiable();
            _dataAccess = new ProductRepository(_context.Object);
        }

        [Test]
        public void Add_Test()
        {
            //arrange
            var product = new Product() { Id = 2, ProductName = "Test" };

            //act
            _dataAccess.AddProduct(product);
            
            //Assert
            _context.Verify(x => x.Products,Times.Exactly(1));
        }
        
        [Test]
        public void Delete_Test() 
        {
            //arrange
            
            //act
            _dataAccess.DeleteProduct(1);

            //Assert
            _context.Verify(x => x.Products, Times.Exactly(2));
        }

        [Test]
        public void GetProducts_Test()
        {
            //arrange

            //act
            _dataAccess.GetProducts();

            //assert
            _context.Verify(x => x.Products, Times.Exactly(1));
        }

        [Test]
        public async Task GetProduct_test()
        {
            //arrange
            
            //act
            var product = await  _dataAccess.GetProduct(1);

            //assert
            Assert.That(product.Id, Is.EqualTo(1));
            Assert.That(product.ProductName, Is.EqualTo("Name"));
            _context.Verify(x => x.Products, Times.Exactly(1));
        }

        [Test]
        public async Task ProductExists_Test()
        {
            //arrange

            //act
            bool exists = await _dataAccess.ProductExists(1);
            bool doesntExists = await _dataAccess.ProductExists(2);

            //assert
            Assert.That(exists, Is.True);
            Assert.That(doesntExists, Is.False);
            _context.Verify(x => x.Products, Times.Exactly(2));
        }

        [Test]
        public void SaveChangesAsync_Test()
        {
            //arrange
            _context.Setup(x => x.SaveChangesAsync()).Verifiable();

            //act
            _dataAccess.SavechangesAsync();

            //assert
            _context.Verify(x => x.SaveChangesAsync(), Times.Exactly(1));
        }
    }
}
