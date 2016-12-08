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
        private List<Product> productData;
        private List<Customer> customerData;
        private List<Address> addressData;
        private Mock<CongoDBEntities> mockDb;
        private Mock<DbSet<Order>> mockSet;

        /// <summary>
        /// Initialize the test data.
        /// </summary>
        public void TestStart() {
            productData = new List<Product> {
                new Product {
                    ProductID = 1,
                    Name = "Product1",
                    Category = new Category(),
                    Active = true
                },
                new Product {
                    ProductID = 2,
                    Name = "Product2",
                    Category = new Category(),
                    Active = true
                },
                new Product {
                    ProductID = 3,
                    Name = "Product3",
                    Category = new Category(),
                    Active = true
                }
            };

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

            addressData = new List<Address> {
                new Address {
                    AddressID = 1,
                    Street = "Street1",
                    Active = true
                },
                new Address {
                    AddressID = 2,
                    Street = "Street2",
                    Active = true
                },
                new Address {
                    AddressID = 3,
                    Street = "Street3",
                    Active = true
                }
            };

            IQueryable<Order> qAccounts = data.AsQueryable();
            IQueryable<Customer> qCustomers = customerData.AsQueryable();
            IQueryable<Product> qProducts = productData.AsQueryable();
            IQueryable<Address> qAddresses = addressData.AsQueryable();

            mockSet = new Mock<DbSet<Order>>();
            mockSet.As<IQueryable<Order>>().Setup(m => m.Provider).Returns(qAccounts.Provider);
            mockSet.As<IQueryable<Order>>().Setup(m => m.Expression).Returns(qAccounts.Expression);
            mockSet.As<IQueryable<Order>>().Setup(m => m.ElementType).Returns(qAccounts.ElementType);
            mockSet.As<IQueryable<Order>>().Setup(m => m.GetEnumerator()).Returns(qAccounts.GetEnumerator());
            mockSet.Setup(m => m.Find(It.IsAny<object[]>())).Returns<object[]>(ids => data.FirstOrDefault(d => d.OrderID == (int)ids[0]));

            Mock<DbSet<Customer>> mockCustomerSet = new Mock<DbSet<Customer>>();
            mockCustomerSet.As<IQueryable<Customer>>().Setup(m => m.Provider).Returns(qCustomers.Provider);
            mockCustomerSet.As<IQueryable<Customer>>().Setup(m => m.Expression).Returns(qCustomers.Expression);
            mockCustomerSet.As<IQueryable<Customer>>().Setup(m => m.ElementType).Returns(qCustomers.ElementType);
            mockCustomerSet.As<IQueryable<Customer>>().Setup(m => m.GetEnumerator()).Returns(qCustomers.GetEnumerator());
            mockCustomerSet.Setup(m => m.Find(It.IsAny<object[]>())).Returns<object[]>(ids => customerData.FirstOrDefault(d => d.CustomerID == (int)ids[0]));

            Mock<DbSet<Product>> mockProductSet = new Mock<DbSet<Product>>();
            mockProductSet.As<IQueryable<Product>>().Setup(m => m.Provider).Returns(qProducts.Provider);
            mockProductSet.As<IQueryable<Product>>().Setup(m => m.Expression).Returns(qProducts.Expression);
            mockProductSet.As<IQueryable<Product>>().Setup(m => m.ElementType).Returns(qProducts.ElementType);
            mockProductSet.As<IQueryable<Product>>().Setup(m => m.GetEnumerator()).Returns(qProducts.GetEnumerator());
            mockProductSet.Setup(m => m.Find(It.IsAny<object[]>())).Returns<object[]>(ids => productData.FirstOrDefault(d => d.ProductID == (int)ids[0]));

            Mock<DbSet<Address>> mockAddressSet = new Mock<DbSet<Address>>();
            mockAddressSet.As<IQueryable<Address>>().Setup(m => m.Provider).Returns(qAddresses.Provider);
            mockAddressSet.As<IQueryable<Address>>().Setup(m => m.Expression).Returns(qAddresses.Expression);
            mockAddressSet.As<IQueryable<Address>>().Setup(m => m.ElementType).Returns(qAddresses.ElementType);
            mockAddressSet.As<IQueryable<Address>>().Setup(m => m.GetEnumerator()).Returns(qAddresses.GetEnumerator());
            mockAddressSet.Setup(m => m.Find(It.IsAny<object[]>())).Returns<object[]>(ids => addressData.FirstOrDefault(d => d.AddressID == (int)ids[0]));

            mockDb = new Mock<CongoDBEntities>();
            mockDb.Setup(db => db.Orders).Returns(mockSet.Object);
            mockDb.Setup(db => db.Customers).Returns(mockCustomerSet.Object);
            mockDb.Setup(db => db.Products).Returns(mockProductSet.Object);
            mockDb.Setup(db => db.Addresses).Returns(mockAddressSet.Object);
            mockDb.Setup(db => db.SaveChanges()).Returns(1);
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

        /// <summary>
        /// Make sure that all of the Orders can be list.
        /// </summary>
        [Fact]
        public void Test_ListOrders() {
            TestStart();

            EfCongoRepository repository = new EfCongoRepository(mockDb.Object);

            OrderController controller = new OrderController(repository);
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            List<Client.Models.Order> response = serializer.Deserialize<List<Client.Models.Order>>(controller.List().Content.ReadAsStringAsync().Result);

            Assert.Equal(3, response.ToList().Count);
            Assert.Equal(1, response[0].OrderID);
            Assert.Equal(2, response[1].OrderID);
            Assert.Equal(3, response[2].OrderID);
        }

        /// <summary>
        /// Make sure that an Order can be created.
        /// </summary>
        [Fact]
        public void Test_CreateOrder_Success() {
            TestStart();

            EfCongoRepository repository = new EfCongoRepository(mockDb.Object);

            OrderController controller = new OrderController(repository);
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            Client.Models.PostResponseBody response = serializer.Deserialize<Client.Models.PostResponseBody>(controller.Post(
                new Client.Models.OrderRequest {
                    AddressID = 1,
                    CustomerID = 1,
                    StripeID = "or_1",
                    ProductIDs = new List<int> { 1, 3 }
                }
            ).Content.ReadAsStringAsync().Result);

            List<Order> orders = mockDb.Object.Orders.ToList();

            Assert.True(response.Success);
            mockSet.Verify(m => m.Add(It.IsAny<Order>()), Times.Once());
            mockDb.Verify(db => db.SaveChanges(), Times.Once());
        }

        /// <summary>
        /// Make sure that the proper response message is returned and the database
        /// is unchanged if the Address does not exist.
        /// </summary>
        [Fact]
        public void Test_CreateOrder_AddressNotFound() {
            TestStart();

            EfCongoRepository repository = new EfCongoRepository(mockDb.Object);

            OrderController controller = new OrderController(repository);
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            Client.Models.PostResponseBody response = serializer.Deserialize<Client.Models.PostResponseBody>(controller.Post(
                new Client.Models.OrderRequest {
                    AddressID = 6,
                    CustomerID = 1,
                    StripeID = "or_1",
                    ProductIDs = new List<int> { 1, 3 }
                }
            ).Content.ReadAsStringAsync().Result);

            List<Order> orders = mockDb.Object.Orders.ToList();

            Assert.False(response.Success);
            Assert.Equal("Address with ID 6 was not found.", response.Message);
            mockSet.Verify(m => m.Add(It.IsAny<Order>()), Times.Never());
            mockDb.Verify(db => db.SaveChanges(), Times.Never());
        }

        /// <summary>
        /// Make sure that the proper response message is returned and the database
        /// is unchanged if the Customer does not exist.
        /// </summary>
        [Fact]
        public void Test_CreateOrder_CustomerNotFound() {
            TestStart();

            EfCongoRepository repository = new EfCongoRepository(mockDb.Object);

            OrderController controller = new OrderController(repository);
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            Client.Models.PostResponseBody response = serializer.Deserialize<Client.Models.PostResponseBody>(controller.Post(
                new Client.Models.OrderRequest {
                    AddressID = 1,
                    CustomerID = 4,
                    StripeID = "or_1",
                    ProductIDs = new List<int> { 1, 3 }
                }
            ).Content.ReadAsStringAsync().Result);

            List<Order> orders = mockDb.Object.Orders.ToList();

            Assert.False(response.Success);
            Assert.Equal("Customer with ID 4 was not found.", response.Message);
            mockSet.Verify(m => m.Add(It.IsAny<Order>()), Times.Never());
            mockDb.Verify(db => db.SaveChanges(), Times.Never());
        }

        /// <summary>
        /// Make sure that the proper response message is returned and the database
        /// is unchanged if a Product does not exist.
        /// </summary>
        [Fact]
        public void Test_CreateOrder_ProductNotFound() {
            TestStart();

            EfCongoRepository repository = new EfCongoRepository(mockDb.Object);

            OrderController controller = new OrderController(repository);
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            Client.Models.PostResponseBody response = serializer.Deserialize<Client.Models.PostResponseBody>(controller.Post(
                new Client.Models.OrderRequest {
                    AddressID = 1,
                    CustomerID = 1,
                    StripeID = "or_1",
                    ProductIDs = new List<int> { 1, 17 }
                }
            ).Content.ReadAsStringAsync().Result);

            List<Order> orders = mockDb.Object.Orders.ToList();

            Assert.False(response.Success);
            Assert.Equal("Product with ID 17 was not found.", response.Message);
            mockSet.Verify(m => m.Add(It.IsAny<Order>()), Times.Never());
            mockDb.Verify(db => db.SaveChanges(), Times.Never());
        }
    }
}
