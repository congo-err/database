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
    /// Tests for Account models and Controller.
    /// </summary>
    public class AccountTests {
        private List<Account> data;
        private Mock<CongoDBEntities> mockDb;

        /// <summary>
        /// Create the AccountTests object and initialize dummy data.
        /// </summary>
        public AccountTests() {
            data = new List<Account> {
                new Account {
                    AccountID = 1,
                    Username = "Account1",
                    Password = "123",
                    Active = true
                },
                new Account {
                    AccountID = 2,
                    Username = "Account2",
                    Password = "456",
                    Active = true
                },
                new Account {
                    AccountID = 3,
                    Username = "Account3",
                    Password = "789",
                    Active = true
                }
            };
        }

        /// <summary>
        /// Initialize the test data.
        /// </summary>
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

        /// <summary>
        /// Test to make sure that we can find an Account by username.
        /// </summary>
        [Fact]
        public void Test_GetAccountByUsername_Success() {
            TestStart();

            EfCongoRepository repository = new EfCongoRepository(mockDb.Object);
            Account account = repository.GetAccountByUsername("Account2");

            Assert.NotNull(account);
            Assert.Equal(2, account.AccountID);
        }

        /// <summary>
        /// Test to make sure null is returned if an Account with the username does not exist.
        /// </summary>
        [Fact]
        public void Test_GetAccountByUsername_Failure() {
            TestStart();

            EfCongoRepository repository = new EfCongoRepository(mockDb.Object);
            Account account = repository.GetAccountByUsername("Account5");

            Assert.Null(account);
        }

        /// <summary>
        /// Test to make sure that if the correct username and password are passed in to return true and the Account.
        /// </summary>
        [Fact]
        public void Test_TryLogin_Success() {
            TestStart();

            EfCongoRepository repository = new EfCongoRepository(mockDb.Object);

            AccountController controller = new AccountController(repository);
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            TryLoginResponse response = serializer.Deserialize<TryLoginResponse>(controller.TryLogin(new Client.Models.UsernamePasswordPair {
                Username = "Account2", Password = "456"
            }).Content.ReadAsStringAsync().Result);

            Assert.True(response.Success);
            Assert.NotNull(response.Account);
            Assert.Equal(2, response.Account.AccountID);
        }

        /// <summary>
        /// Test to make sure that false and the correct message is returned if no Account with the given username is found.
        /// </summary>
        [Fact]
        public void Test_TryLogin_IncorrectUsername() {
            TestStart();

            EfCongoRepository repository = new EfCongoRepository(mockDb.Object);

            AccountController controller = new AccountController(repository);
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            TryLoginResponse response = serializer.Deserialize<TryLoginResponse>(controller.TryLogin(new Client.Models.UsernamePasswordPair {
                Username = "Account5",
                Password = "456"
            }).Content.ReadAsStringAsync().Result);

            Assert.False(response.Success);
            Assert.Equal("No user with username Account5 was found.", response.Message);
            Assert.Null(response.Account);
        }

        /// <summary>
        /// Test to make sure that false and the correct message is returned if the password does not match the Account with the given username.
        /// </summary>
        [Fact]
        public void Test_TryLogin_IncorrectPassword() {
            TestStart();

            EfCongoRepository repository = new EfCongoRepository(mockDb.Object);

            AccountController controller = new AccountController(repository);
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            TryLoginResponse response = serializer.Deserialize<TryLoginResponse>(controller.TryLogin(new Client.Models.UsernamePasswordPair {
                Username = "Account2",
                Password = "789"
            }).Content.ReadAsStringAsync().Result);

            Assert.False(response.Success);
            Assert.Equal("The username/password combination was incorrect.", response.Message);
            Assert.Null(response.Account);
        }

        /// <summary>
        /// Response object for the TryLogin POST action.
        /// </summary>
        private class TryLoginResponse {
            public bool Success { get; set; }
            public string Message { get; set; }
            public Client.Models.Account Account { get; set; }
        }
    }
}
