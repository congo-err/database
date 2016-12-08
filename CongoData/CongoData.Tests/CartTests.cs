﻿using CongoData.Client.Controllers;
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
using System.Web.Script.Serialization;
using Xunit;

namespace CongoData.Tests {
    /// <summary>
    /// Tests for Cart models and controllers.
    /// </summary>
    public class CartTests {
        private List<Cart> data;
        private List<Product> productData;
        private List<Customer> customerData;
        private Mock<CongoDBEntities> mockDb;

        /// <summary>
        /// Initialize the test data.
        /// </summary>
        private void TestStart() {
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

            data = new List<Cart> {
                new Cart {
                    CustomerID = 1,
                    Products = new List<Product> {
                        productData[1]
                    },
                    Active = true
                },
                new Cart {
                    CustomerID = 2,
                    Products = new List<Product>(),
                    Active = true
                },
                new Cart {
                    CustomerID = 3,
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

            IQueryable<Cart> qCarts = data.AsQueryable();
            IQueryable<Customer> qCustomers = customerData.AsQueryable();
            IQueryable<Product> qProducts = productData.AsQueryable();

            Mock<DbSet<Cart>> mockSet = new Mock<DbSet<Cart>>();
            mockSet.As<IQueryable<Cart>>().Setup(m => m.Provider).Returns(qCarts.Provider);
            mockSet.As<IQueryable<Cart>>().Setup(m => m.Expression).Returns(qCarts.Expression);
            mockSet.As<IQueryable<Cart>>().Setup(m => m.ElementType).Returns(qCarts.ElementType);
            mockSet.As<IQueryable<Cart>>().Setup(m => m.GetEnumerator()).Returns(qCarts.GetEnumerator());
            mockSet.Setup(m => m.Find(It.IsAny<object[]>())).Returns<object[]>(ids => data.FirstOrDefault(d => d.CustomerID == (int)ids[0]));

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

            mockDb = new Mock<CongoDBEntities>();
            mockDb.Setup(db => db.Carts).Returns(mockSet.Object);
            mockDb.Setup(db => db.Customers).Returns(mockCustomerSet.Object);
            mockDb.Setup(db => db.Products).Returns(mockProductSet.Object);
            mockDb.Setup(db => db.SaveChanges()).Returns(1);
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

        /// <summary>
        /// Make sure that a Product can successfully be added to a Cart if both
        /// the Cart and Product exist.
        /// </summary>
        [Fact]
        public void Test_AddProductToCart_Success() {
            TestStart();

            EfCongoRepository repository = new EfCongoRepository(mockDb.Object);

            CartController controller = new CartController(repository);
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            Client.Models.PostResponseBody response = serializer.Deserialize<Client.Models.PostResponseBody>(controller.Post(new Client.Models.CartProduct {
                CartID = 2,
                ProductID = 1
            }).Content.ReadAsStringAsync().Result);

            Assert.True(response.Success);
            Assert.Equal(1, data[1].Products.ToList().Count);
            Assert.Equal("Product1", data[1].Products.ToList()[0].Name);
        }

        /// <summary>
        /// Make sure that a Product is not added and the correct response message
        /// is returned if the Cart cannot be found.
        /// </summary>
        [Fact]
        public void Test_AddProductToCart_CartNotFound() {
            TestStart();

            EfCongoRepository repository = new EfCongoRepository(mockDb.Object);

            CartController controller = new CartController(repository);
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            Client.Models.PostResponseBody response = serializer.Deserialize<Client.Models.PostResponseBody>(controller.Post(new Client.Models.CartProduct {
                CartID = 4,
                ProductID = 1
            }).Content.ReadAsStringAsync().Result);

            Assert.False(response.Success);
            Assert.Equal("Cart with ID 4 was not found.", response.Message);
            Assert.Equal(0, data[1].Products.ToList().Count);
        }

        /// <summary>
        /// Make sure that a Product is not added and the correct response message
        /// is returned if the Product cannot be found.
        /// </summary>
        [Fact]
        public void Test_AddProductToCart_ProductNotFound() {
            TestStart();

            EfCongoRepository repository = new EfCongoRepository(mockDb.Object);

            CartController controller = new CartController(repository);
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            Client.Models.PostResponseBody response = serializer.Deserialize<Client.Models.PostResponseBody>(controller.Post(new Client.Models.CartProduct {
                CartID = 2,
                ProductID = 5
            }).Content.ReadAsStringAsync().Result);

            Assert.False(response.Success);
            Assert.Equal("Product with ID 5 was not found.", response.Message);
            Assert.Equal(0, data[1].Products.ToList().Count);
        }

        /// <summary>
        /// Make sure that a Product can successfully be removed to a Cart if both
        /// the Cart and Product exist and the Product exists in the Cart.
        /// </summary>
        [Fact]
        public void Test_RemoveProductFromCart_Success() {
            TestStart();

            EfCongoRepository repository = new EfCongoRepository(mockDb.Object);

            CartController controller = new CartController(repository);
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            Client.Models.PostResponseBody response = serializer.Deserialize<Client.Models.PostResponseBody>(controller.Delete(1, 2).Content.ReadAsStringAsync().Result);

            Assert.True(response.Success);
            Assert.Equal(0, data[0].Products.ToList().Count);
        }

        /// <summary>
        /// Make sure that a Product is not removed and the correct response message
        /// is returned if the Cart cannot be found.
        /// </summary>
        [Fact]
        public void Test_RemoveProductFromCart_CartNotFound() {
            TestStart();

            EfCongoRepository repository = new EfCongoRepository(mockDb.Object);

            CartController controller = new CartController(repository);
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            Client.Models.PostResponseBody response = serializer.Deserialize<Client.Models.PostResponseBody>(controller.Delete(4, 2).Content.ReadAsStringAsync().Result);

            Assert.False(response.Success);
            Assert.Equal("Cart with ID 4 was not found.", response.Message);
            Assert.Equal(1, data[0].Products.ToList().Count);
        }

        /// <summary>
        /// Make sure that a Product is not removed and the correct response message
        /// is returned if the Product cannot be found.
        /// </summary>
        [Fact]
        public void Test_RemoveProductFromCart_ProductNotFound() {
            TestStart();

            EfCongoRepository repository = new EfCongoRepository(mockDb.Object);

            CartController controller = new CartController(repository);
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            Client.Models.PostResponseBody response = serializer.Deserialize<Client.Models.PostResponseBody>(controller.Delete(1, 5).Content.ReadAsStringAsync().Result);

            Assert.False(response.Success);
            Assert.Equal("Product with ID 5 was not found.", response.Message);
            Assert.Equal(1, data[0].Products.ToList().Count);
        }

        /// <summary>
        /// Make sure that a Product cannot be removed if it isn't in the Cart
        /// and the proper response Message is returned.
        /// </summary>
        [Fact]
        public void Test_RemoveProductFromCart_ProductNotInCart() {
            TestStart();

            EfCongoRepository repository = new EfCongoRepository(mockDb.Object);

            CartController controller = new CartController(repository);
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            Client.Models.PostResponseBody response = serializer.Deserialize<Client.Models.PostResponseBody>(controller.Delete(1, 3).Content.ReadAsStringAsync().Result);

            Assert.False(response.Success);
            Assert.Equal("Product is not in the cart.", response.Message);
            Assert.Equal(1, data[0].Products.ToList().Count);
        }

        /// <summary>
        /// Make sure that a Cart can be cleared if it exists.
        /// </summary>
        [Fact]
        public void Test_ClearCart_Success() {
            TestStart();

            EfCongoRepository repository = new EfCongoRepository(mockDb.Object);

            CartController controller = new CartController(repository);
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            Client.Models.PostResponseBody response = serializer.Deserialize<Client.Models.PostResponseBody>(controller.Clear(1).Content.ReadAsStringAsync().Result);

            Assert.True(response.Success);
            Assert.Equal(0, data[0].Products.ToList().Count);
        }

        /// <summary>
        /// Make sure that the proper message is returned if a Cart that does
        /// not exist is attempted to be cleared.
        /// </summary>
        [Fact]
        public void Test_ClearCart_CartNotFound() {
            TestStart();

            EfCongoRepository repository = new EfCongoRepository(mockDb.Object);

            CartController controller = new CartController(repository);
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            Client.Models.PostResponseBody response = serializer.Deserialize<Client.Models.PostResponseBody>(controller.Clear(4).Content.ReadAsStringAsync().Result);

            Assert.False(response.Success);
            Assert.Equal("Cart with ID 4 was not found.", response.Message);
        }
    }
}
