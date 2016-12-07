using CongoData.DataAccess.Abstract;
using System.Collections.Generic;
using System.Linq;

namespace CongoData.DataAccess.Concrete {
    /// <summary>
    /// An implementation of the ICongoRepository which returns data from the
    /// Entity Framework entities.
    /// </summary>
    public class EfCongoRepository : ICongoRepository {
        private readonly CongoDBEntities data;

        /// <summary>
        /// Create a new EfCongoRepository.
        /// </summary>
        public EfCongoRepository() {
            data = new CongoDBEntities();
        }

        /// <summary>
        /// Create a new EfCongoRepository, passing in a specific Entity Framework context.
        /// </summary>
        /// <param name="data">The CongoDBEntities EF context.</param>
        public EfCongoRepository(CongoDBEntities data) {
            this.data = data;
        }

        /// <summary>
        /// Get an Account.
        /// </summary>
        /// <param name="id">The ID of the Account.</param>
        /// <returns>The Account object or null if it was not found.</returns>
        public Account GetAccount(int id) {
            Account a = data.Accounts.Find(id);

            if (a != null && a.Active) {
                return a;
            }

            return null;
        }

        /// <summary>
        /// Get an Account by username.
        /// </summary>
        /// <param name="username">The username of the Account.</param>
        /// <returns>The Account object or null if it was not found.</returns>
        public Account GetAccountByUsername(string username) {
            Account a = data.Accounts.FirstOrDefault(d => d.Username == username);

            if (a != null && a.Active) {
                return a;
            }

            return null;
        }

        /// <summary>
        /// Get a Cart.
        /// </summary>
        /// <param name="id">The ID of the Cart.</param>
        /// <returns>The Cart object or null if it was not found.</returns>
        public Cart GetCart(int id) {
            Cart c = data.Carts.Find(id);

            if (c != null && c.Active) {
                return c;
            }

            return null;
        }

        /// <summary>
        /// List all of the Categories.
        /// </summary>
        /// <returns>The List of Categories.</returns>
        public List<Category> ListCategories() {
            return data.Categories.Where(c => c.Active).ToList();
        }

        /// <summary>
        /// Get a Customer.
        /// </summary>
        /// <param name="id">The ID of the Customer.</param>
        /// <returns>The Customer object or null if it was not found.</returns>
        public Customer GetCustomer(int id) {
            Customer c = data.Customers.Find(id);

            if (c != null && c.Active) {
                return c;
            }

            return null;
        }
    }
}
