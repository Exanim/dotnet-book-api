using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using Products.Api.Controllers;
using Products.Api.Entities;
using Products.Api.Services;
using Products.Api.Models;
using System.Diagnostics.Contracts;
using Microsoft.AspNetCore.Http;

namespace ProdutApi.Tests
{
    public class ProductApiController_Tests
    {
        private static Mock<IProductRepository> _productRepository;
        private static Mock<AutoMapper.IMapper> _mapper;
        private static ProductApiController _productApiController;

        [SetUp] 
        public void SetUp() {
        _productRepository = new Mock<IProductRepository>();
        _mapper = new Mock<AutoMapper.IMapper>();
        _productApiController = new ProductApiController(_productRepository.Object, _mapper.Object);
        }

        [Test]
        public async Task AddProduct_Test()
        {
            //arrange
            var product = new ProductDto()
            {
                ProductName = "TestName",
            };
            _mapper.Setup(x => x.Map<Product>(It.Is<ProductDto>(p => p.ProductName == "TestName")))
                .Returns(new Product() { Id = 1, ProductName = "TestName" }).Verifiable();
            _productRepository.Setup(x => x.AddProduct(It.IsAny<Product>())).Verifiable();



            //act
            await _productApiController.AddProduct(product);

            //assert
            _productRepository.Verify(x => x.AddProduct(It.IsAny<Product>()));
            _mapper.Verify();
        }

        [Test]
        public async Task GetProductById_Test()
        {
            //arrange 
            _mapper.Setup(x => x.Map<Product>(It.IsAny<ProductDto>()))
                .Returns(new Product() { Id = 1, ProductName = "TestName" }).Verifiable();
            _productRepository.Setup(x => x.ProductExists(It.Is<int>(n => n != 0))).ReturnsAsync(true).Verifiable();
            _productRepository.Setup(x => x.GetProduct(It.Is<int>(n => n != 0))).ReturnsAsync(new Product() { Id = 1, ProductName = "TestName" }).Verifiable();


            //act
            var notFoundResult = await _productApiController.GetProductById(0) as NotFoundResult;
            var okResult = await _productApiController.GetProductById(1) as OkObjectResult;

            //assert
            _productRepository.Verify(x => x.GetProduct(It.Is<int>(n => n != 0)),Times.Exactly(1));
            _productRepository.Verify(x => x.ProductExists(It.Is<int>(n => n != 0)), Times.Exactly(1));
            Assert.That(notFoundResult.StatusCode, Is.EqualTo(404));
            Assert.That(okResult.StatusCode, Is.EqualTo(200));
        }

        [Test]
        public async Task GetProducts_Test()
        {
            //arrange
            _productRepository.Setup(x => x.GetProducts()).Verifiable();

            //act
            var results = await _productApiController.GetProducts();

            //assert
            _productRepository.Verify();
            _mapper.Verify();
        }

        [Test]
        public async Task DeleteProduct_Test()
        {
            //arrange
            _productRepository.Setup(x => x.DeleteProduct(It.Is<int>(n => n != 0))).Returns(true).Verifiable();
            _productRepository.Setup(x => x.DeleteProduct(It.Is<int>(n => n == 0))).Returns(false).Verifiable();

            //act
            var notFoundResult = await _productApiController.DeleteProduct(0) as NotFoundResult;
            var noContentResult = await _productApiController.DeleteProduct(1) as NoContentResult;

            //assert
            Assert.That(notFoundResult.StatusCode, Is.EqualTo(404));
            Assert.That(noContentResult.StatusCode, Is.EqualTo(204));
            _productRepository.Verify(x => x.DeleteProduct(It.Is<int>(n => n != 0)), Times.Exactly(1));
            _productRepository.Verify(x => x.DeleteProduct(It.Is<int>(n => n == 0)), Times.Exactly(1));

        }
    }
}