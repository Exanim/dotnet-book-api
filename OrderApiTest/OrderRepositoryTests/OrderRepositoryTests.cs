using Moq;
using Moq.EntityFrameworkCore;
using NUnit.Framework;
using OrderApi.DbContexts;
using OrderApi.Entities;
using OrderApi.Services;

namespace OrderApi.Tests.OrderRepositoryTests
{
    public class OrderRepositoryTests
    {
        private Mock<IOrdersContext> _context;
        private OrdersRepository ordersRepository;         

        [SetUp]
        public void SetUp()
        {
            _context = new Mock<IOrdersContext>();
            List<OrderEntity> orders = new List<OrderEntity>() {new OrderEntity() 
                { OrderId = 1, UserId = 3, ProductIds = new List<ProductEntity>(){ new ProductEntity() { Id = 3, productId = 4, OrderId = 1 } } 
                } 
            };
            _context.Setup(x => x.Orders).ReturnsDbSet(orders).Verifiable();
            ordersRepository = new OrdersRepository(_context.Object);
        }

        [Test]
        public void AddOrder_Calls_Context()
        {
            //arrange
            OrderEntity order = new OrderEntity();
            
            //act
            ordersRepository.AddOrder(order);

            //assert
            _context.Verify(x => x.Orders, Times.Once());
        }

        [Test]
        public async Task GetAllOrdersAsync_Reads_Context()
        {
            //arrange

            //act
            var orders = await ordersRepository.GetAllOrdersAsync();

            //assert
            _context.Verify(x => x.Orders, Times.Once());
            Assert.That(orders.Count, Is.EqualTo(1));
            Assert.That(orders.ElementAt(0).OrderId, Is.EqualTo(1));
        }

        [Test]
        public async Task GetOrderByIdAsync_Reads_Context()
        {
            //arrange

            //act
            var order = await ordersRepository.GetOrderByIdAsync(1);

            //assert
            _context.Verify(x => x.Orders, Times.Once());
            
            Assert.That(order.OrderId, Is.EqualTo(1));
        }

        [Test]
        public void Savechanges_Calls_Context()
        {
            //arramge
            _context.Setup(x => x.SaveChanges()).Verifiable();

            //act
            ordersRepository.Savechanges();

            //assert
            _context.Verify(x => x.SaveChanges(),Times.Once);
        }

        [Test]
        public void DeleteOrder_Calls_Context()
        {
            //arrange
            OrderEntity order = new OrderEntity();

            //act
            ordersRepository.DeleteOrder(order);

            //assert
            _context.Verify(x => x.Orders, Times.Once());
        }
    }
}
