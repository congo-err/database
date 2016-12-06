using CongoData.Client.Controllers;
using CongoData.DataAccess;
using CongoData.DataAccess.Concrete;
using Moq;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Xunit;

namespace CongoData.Tests {
    /// <summary>
    /// Tests for Cart models and controllers.
    /// </summary>
    public class CartTests {
        private List<Cart> data;
        private List<Customer> customerData;
        private Mock<CongoDBEntities> mockDb;

        /// <summary>
        /// Create the CartTests object and initialize dummy data.
        /// </summary>
        public CartTests() {
            data = new List<Cart> {
                new Cart {
                    CustomerID = 1,
                    Active = true
                },
                new Cart {
                    CustomerID = 2,
                    Active = true
                },
                new Cart {
                    CustomerID = 3,
                    Active = true
                }
            };

            customerData = new List<Customer> {
                new Customer {
                    CustomerID = 1,
                    Account = new Account(),
                    Address = new Address(),
                    Active = true
                },
                new Customer {
                    CustomerID = 2,
                    Account = new Account(),
                    Address = new Address(),
                    Active = true
                },
                new Customer {
                    CustomerID = 3,
                    Account = new Account(),
                    Address = new Address(),
                    Active = true
                }
            };
        }

        /// <summary>
        /// Initialize the test data.
        /// </summary>
        private void TestStart() {
            IQueryable<Cart> qCarts = data.AsQueryable();

            Mock<DbSet<Cart>> mockSet = new Mock<DbSet<Cart>>();
            mockSet.As<IQueryable<Cart>>().Setup(m => m.Provider).Returns(qCarts.Provider);
            mockSet.As<IQueryable<Cart>>().Setup(m => m.Expression).Returns(qCarts.Expression);
            mockSet.As<IQueryable<Cart>>().Setup(m => m.ElementType).Returns(qCarts.ElementType);
            mockSet.As<IQueryable<Cart>>().Setup(m => m.GetEnumerator()).Returns(qCarts.GetEnumerator());
            mockSet.Setup(m => m.Find(It.IsAny<object[]>())).Returns<object[]>(ids => data.FirstOrDefault(d => d.CustomerID == (int)ids[0]));

            Mock<DbSet<Customer>> mockCustomerSet = new Mock<DbSet<Customer>>();
            mockCustomerSet.As<IQueryable<Cart>>().Setup(m => m.Provider).Returns(qCarts.Provider);
            mockCustomerSet.As<IQueryable<Cart>>().Setup(m => m.Expression).Returns(qCarts.Expression);
            mockCustomerSet.As<IQueryable<Cart>>().Setup(m => m.ElementType).Returns(qCarts.ElementType);
            mockCustomerSet.As<IQueryable<Cart>>().Setup(m => m.GetEnumerator()).Returns(qCarts.GetEnumerator());
            mockCustomerSet.Setup(m => m.Find(It.IsAny<object[]>())).Returns<object[]>(ids => customerData.FirstOrDefault(d => d.CustomerID == (int)ids[0]));

            mockDb = new Mock<CongoDBEntities>();
            mockDb.Setup(db => db.Carts).Returns(mockSet.Object);
            mockDb.Setup(db => db.Customers).Returns(mockCustomerSet.Object);
        }

        /// <summary>
        /// Test to make sure that the repository successfully returns an Account object if it exists.
        /// </summary>
        [Fact]
        public void Test_GetCart_Success() {
            TestStart();

            EfCongoRepository repository = new EfCongoRepository(mockDb.Object);
            Cart cart = repository.GetCart(2);

            CartController controller = new CartController(repository);
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            Assert.NotNull(cart);
            Assert.Equal(2, cart.CustomerID);
            Assert.Equal(HttpStatusCode.OK, controller.Get(2).StatusCode);
        }

        /// <summary>
        /// Test to make sure that the repository successfully returns null if it doesn't exist.
        /// </summary>
        [Fact]
        public void Test_GetCart_Failure() {
            TestStart();

            EfCongoRepository repository = new EfCongoRepository(mockDb.Object);
            Cart cart = repository.GetCart(5);

            CartController controller = new CartController(repository);
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            Assert.Null(cart);
            Assert.Equal(HttpStatusCode.NotFound, controller.Get(5).StatusCode);
        }
    }
}
