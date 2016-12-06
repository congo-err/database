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
    public class AccountTests {
        private List<Account> data;
        private Mock<CongoDBEntities> mockDb;

        public AccountTests() {
            data = new List<Account> {
                new Account {
                    AccountID = 1,
                    Username = "Account1"
                },
                new Account {
                    AccountID = 2,
                    Username = "Account2"
                },
                new Account {
                    AccountID = 3,
                    Username = "Account3"
                }
            };
        }

        private void TestStart() {
            IQueryable<Account> qAccounts = data.AsQueryable();
            
            Mock<DbSet<Account>> mockSet = new Mock<DbSet<Account>>();
            mockSet.As<IQueryable<Account>>().Setup(m => m.Provider).Returns(qAccounts.Provider);
            mockSet.As<IQueryable<Account>>().Setup(m => m.Expression).Returns(qAccounts.Expression);
            mockSet.As<IQueryable<Account>>().Setup(m => m.ElementType).Returns(qAccounts.ElementType);
            mockSet.As<IQueryable<Account>>().Setup(m => m.GetEnumerator()).Returns(qAccounts.GetEnumerator());
            mockSet.Setup(m => m.Find(It.IsAny<object[]>())).Returns<object[]>(ids => data.FirstOrDefault(d => d.AccountID == (int)ids[0]));

            mockDb = new Mock<CongoDBEntities>();
            mockDb.Setup(db => db.Accounts).Returns(mockSet.Object);
        }

        /// <summary>
        /// Test to make sure that the repository successfully returns an Account object if it exists.
        /// </summary>
        [Fact]
        public void Test_GetAccount_Success() {
            TestStart();

            EfCongoRepository repository = new EfCongoRepository(mockDb.Object);
            Account account = repository.GetAccount(2);

            AccountController controller = new AccountController(repository);
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            Assert.NotNull(account);
            Assert.Equal(2, account.AccountID);
            Assert.Equal(HttpStatusCode.OK, controller.Get(2).StatusCode);
        }

        /// <summary>
        /// Test to make sure that the repository successfully returns null if it doesn't exist.
        /// </summary>
        [Fact]
        public void Test_GetAccount_Failure() {
            TestStart();

            EfCongoRepository repository = new EfCongoRepository(mockDb.Object);
            Account account = repository.GetAccount(5);

            AccountController controller = new AccountController(repository);
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            Assert.Null(account);
            Assert.Equal(HttpStatusCode.NotFound, controller.Get(5).StatusCode);
        }
    }
}
