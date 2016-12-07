using CongoData.Client.Controllers;
using CongoData.DataAccess;
using CongoData.DataAccess.Concrete;
using Moq;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Script.Serialization;
using Xunit;

namespace CongoData.Tests {
    /// <summary>
    /// Tests for Order models and Controller.
    /// </summary>
    public class OrderTests {
        private List<Order> data;
        private Mock<CongoDBEntities> mockDb;

        /// <summary>
        /// Initialize the test data.
        /// </summary>
        public void TestStart() {
            data = new List<Order> {
                new Order {
                    OrderID = 1,
                    CustomerID = 1,
                    Customer = new Customer {
                        CustomerID = 1,
                        Name = "Customer1",
                        Account = new Account(),
                        Address = new Address(),
                        Active = true
                    },
                    Address = new Address(),
                    Products = new List<Product>(),
                    Active = true
                },
                new Order {
                    OrderID = 2,
                    CustomerID = 2,
                    Customer = new Customer {
                        CustomerID = 2,
                        Name = "Customer2",
                        Account = new Account(),
                        Address = new Address(),
                        Active = true
                    },
                    Address = new Address(),
                    Products = new List<Product>(),
                    Active = true
                },
                new Order {
                    OrderID = 3,
                    CustomerID = 1,
                    Customer = new Customer {
                        CustomerID = 1,
                        Name = "Customer1",
                        Account = new Account(),
                        Address = new Address(),
                        Active = true
                    },
                    Address = new Address(),
                    Products = new List<Product>(),
                    Active = true
                }
            };

            IQueryable<Order> qAccounts = data.AsQueryable();

            Mock<DbSet<Order>> mockSet = new Mock<DbSet<Order>>();
            mockSet.As<IQueryable<Order>>().Setup(m => m.Provider).Returns(qAccounts.Provider);
            mockSet.As<IQueryable<Order>>().Setup(m => m.Expression).Returns(qAccounts.Expression);
            mockSet.As<IQueryable<Order>>().Setup(m => m.ElementType).Returns(qAccounts.ElementType);
            mockSet.As<IQueryable<Order>>().Setup(m => m.GetEnumerator()).Returns(qAccounts.GetEnumerator());
            mockSet.Setup(m => m.Find(It.IsAny<object[]>())).Returns<object[]>(ids => data.FirstOrDefault(d => d.OrderID == (int)ids[0]));

            mockDb = new Mock<CongoDBEntities>();
            mockDb.Setup(db => db.Orders).Returns(mockSet.Object);
        }
        
        /// <summary>
        /// Make sure that we can get a list of all Orders made
        /// by a Customer.
        /// </summary>
        [Fact]
        public void Test_ListOrdersFromCustomer() {
            TestStart();

            EfCongoRepository repository = new EfCongoRepository(mockDb.Object);

            OrderController controller = new OrderController(repository);
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            List<Client.Models.Order> response = serializer.Deserialize<List<Client.Models.Order>>(controller.List(1).Content.ReadAsStringAsync().Result);
            
            Assert.Equal(2, response.ToList().Count);
            Assert.Equal(1, response[0].OrderID);
            Assert.Equal(3, response[1].OrderID);
        }
    }
}
