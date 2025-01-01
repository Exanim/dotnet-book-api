using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using NUnit.Framework;
using Products.Api.Entities;

using Products.Api.Models;
using Products.Api.Profiles;



namespace ProdutApi.Tests
{
    public class ProductApiMapper_Tests
    {
        private static Product product = new Product() { Id = 1, ProductName = "Name" };
        private static ProductDto productDto = new ProductDto() {ProductName = "Name" };
        private static ProductWithIdDto productWithIdDto = new ProductWithIdDto() { ProductId = 1, Product = productDto };
        private static IMapper mapper;
        [SetUp]
        public void SetUp()
        {
            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new ProductProfile());
            });

            mapper = mapperConfig.CreateMapper();
        }

        [Test]
        public void Mapper_Test()
        {
            //arrange
            ProductWithIdDto productWithIdDtoFromProduct;
            ProductDto productDtoFromProduct;
            Product productFromProductDto;

            //act
            productWithIdDtoFromProduct = mapper.Map<ProductWithIdDto>(product);
            productDtoFromProduct = mapper.Map<ProductDto>(product);
            productFromProductDto = mapper.Map<Product>(productDto);

            //assert
            Assert.That(productWithIdDtoFromProduct.ProductId, Is.EqualTo(product.Id));
            Assert.That(productWithIdDtoFromProduct.Product.ProductName, Is.EqualTo(product.ProductName));
            Assert.That(productDtoFromProduct.ProductName, Is.EqualTo(product.ProductName));
            Assert.That(productFromProductDto.ProductName, Is.EqualTo(productDto.ProductName));
        }



    }
}
