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
    /// Tests for Category models and Controller.
    /// </summary>
    public class CategoryTests {
        private List<Category> data;
        private List<Product> productData;
        private Mock<CongoDBEntities> mockDb;

        /// <summary>
        /// Create the CategoryTests object and initialize dummy data.
        /// </summary>
        public CategoryTests() {
            data = new List<Category> {
                new Category {
                    CategoryID = 1,
                    Name = "Category1",
                    Active = true
                },
                new Category {
                    CategoryID = 2,
                    Name = "Category2",
                    Active = false
                },
                new Category {
                    CategoryID = 3,
                    Name = "Category3",
                    Active = true
                }
            };

            productData = new List<Product> {
                new Product {
                    ProductID = 1,
                    Name = "Product1",
                    CategoryID = 1,
                    Category = data[0],
                    Active = true
                },
                new Product {
                    ProductID = 2,
                    Name = "Product2",
                    CategoryID = 3,
                    Category = data[2],
                    Active = true
                },
                new Product {
                    ProductID = 3,
                    Name = "Product3",
                    CategoryID = 1,
                    Category = data[0],
                    Active = true
                }
            };
        }

        /// <summary>
        /// Initialize the test data.
        /// </summary>
        private void TestStart() {
            IQueryable<Category> qAccounts = data.AsQueryable();
            IQueryable<Product> qProducts = productData.AsQueryable();

            Mock<DbSet<Category>> mockSet = new Mock<DbSet<Category>>();
            mockSet.As<IQueryable<Category>>().Setup(m => m.Provider).Returns(qAccounts.Provider);
            mockSet.As<IQueryable<Category>>().Setup(m => m.Expression).Returns(qAccounts.Expression);
            mockSet.As<IQueryable<Category>>().Setup(m => m.ElementType).Returns(qAccounts.ElementType);
            mockSet.As<IQueryable<Category>>().Setup(m => m.GetEnumerator()).Returns(qAccounts.GetEnumerator());
            mockSet.Setup(m => m.Find(It.IsAny<object[]>())).Returns<object[]>(ids => data.FirstOrDefault(d => d.CategoryID == (int)ids[0]));

            Mock<DbSet<Product>> mockProductSet = new Mock<DbSet<Product>>();
            mockProductSet.As<IQueryable<Product>>().Setup(m => m.Provider).Returns(qProducts.Provider);
            mockProductSet.As<IQueryable<Product>>().Setup(m => m.Expression).Returns(qProducts.Expression);
            mockProductSet.As<IQueryable<Product>>().Setup(m => m.ElementType).Returns(qProducts.ElementType);
            mockProductSet.As<IQueryable<Product>>().Setup(m => m.GetEnumerator()).Returns(qProducts.GetEnumerator());
            mockProductSet.Setup(m => m.Find(It.IsAny<object[]>())).Returns<object[]>(ids => productData.FirstOrDefault(d => d.ProductID == (int)ids[0]));

            mockDb = new Mock<CongoDBEntities>();
            mockDb.Setup(db => db.Categories).Returns(mockSet.Object);
            mockDb.Setup(db => db.Products).Returns(mockProductSet.Object);
        }

        /// <summary>
        /// Test to make sure the API returns a List of all active Categories.
        /// </summary>
        [Fact]
        public void Test_ListCategories() {
            TestStart();

            EfCongoRepository repository = new EfCongoRepository(mockDb.Object);

            CategoryController controller = new CategoryController(repository);
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            HttpResponseMessage categories = controller.List();

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            List<Client.Models.Category> response = serializer.Deserialize<List<Client.Models.Category>>(controller.List().Content.ReadAsStringAsync().Result);

            Assert.Equal(2, response.Count);
            Assert.Equal("Category1", response[0].Name);
            Assert.Equal("Category3", response[1].Name);
        }

        /// <summary>
        /// Make sure that a list of all Products in a Category.
        /// </summary>
        [Fact]
        public void Test_ListProductsInCategory() {
            TestStart();

            EfCongoRepository repository = new EfCongoRepository(mockDb.Object);

            CategoryController controller = new CategoryController(repository);
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            HttpResponseMessage categories = controller.List();

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            List<Client.Models.Product> response = serializer.Deserialize<List<Client.Models.Product>>(controller.Get(1).Content.ReadAsStringAsync().Result);

            Assert.Equal(2, response.Count);
            Assert.Equal("Product1", response[0].Name);
            Assert.Equal("Product3", response[1].Name);
        }
    }
}
