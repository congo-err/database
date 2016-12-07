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
using System.Web.Script.Serialization;
using Xunit;

namespace CongoData.Tests {
    /// <summary>
    /// Tests for Product models and controllers.
    /// </summary>
    public class ProductTests {
        private List<Product> data;
        private Mock<CongoDBEntities> mockDb;

        /// <summary>
        /// Create the ProductTests object and initialize dummy data.
        /// </summary>
        public ProductTests() {
            data = new List<Product> {
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
                    Active = false
                },
                new Product {
                    ProductID = 3,
                    Name = "Product3",
                    Category = new Category(),
                    Active = true
                }
            };
        }

        /// <summary>
        /// Initialize the test data.
        /// </summary>
        private void TestStart() {
            IQueryable<Product> qProducts = data.AsQueryable();

            Mock<DbSet<Product>> mockSet = new Mock<DbSet<Product>>();
            mockSet.As<IQueryable<Product>>().Setup(m => m.Provider).Returns(qProducts.Provider);
            mockSet.As<IQueryable<Product>>().Setup(m => m.Expression).Returns(qProducts.Expression);
            mockSet.As<IQueryable<Product>>().Setup(m => m.ElementType).Returns(qProducts.ElementType);
            mockSet.As<IQueryable<Product>>().Setup(m => m.GetEnumerator()).Returns(qProducts.GetEnumerator());
            mockSet.Setup(m => m.Find(It.IsAny<object[]>())).Returns<object[]>(ids => data.FirstOrDefault(d => d.ProductID == (int)ids[0]));

            mockDb = new Mock<CongoDBEntities>();
            mockDb.Setup(db => db.Products).Returns(mockSet.Object);
        }

        /// <summary>
        /// Test to make sure the API returns a List of all active Products.
        /// </summary>
        [Fact]
        public void Test_ListProducts() {
            TestStart();

            EfCongoRepository repository = new EfCongoRepository(mockDb.Object);

            ProductController controller = new ProductController(repository);
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            HttpResponseMessage categories = controller.List();

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            List<Client.Models.Product> response = serializer.Deserialize<List<Client.Models.Product>>(controller.List().Content.ReadAsStringAsync().Result);

            Assert.Equal(2, response.Count);
            Assert.Equal("Product1", response[0].Name);
            Assert.Equal("Product3", response[1].Name);
        }

        /// <summary>
        /// Test to make sure that the repository successfully returns an Product object if it exists.
        /// </summary>
        [Fact]
        public void Test_GetProduct_Success() {
            TestStart();

            EfCongoRepository repository = new EfCongoRepository(mockDb.Object);
            Product product = repository.GetProduct(3);

            ProductController controller = new ProductController(repository);
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            Assert.NotNull(product);
            Assert.Equal(3, product.ProductID);
            Assert.Equal(HttpStatusCode.OK, controller.Get(3).StatusCode);
        }

        /// <summary>
        /// Test to make sure that the repository successfully returns null if it doesn't exist.
        /// </summary>
        [Fact]
        public void Test_GetProduct_Failure() {
            TestStart();

            EfCongoRepository repository = new EfCongoRepository(mockDb.Object);
            Product product = repository.GetProduct(2);

            ProductController controller = new ProductController(repository);
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            Assert.Null(product);
            Assert.Equal(HttpStatusCode.NotFound, controller.Get(2).StatusCode);
        }
    }
}
