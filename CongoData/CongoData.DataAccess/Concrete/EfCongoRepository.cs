using CongoData.DataAccess.Abstract;

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
            return data.Accounts.Find(id);
        }
    }
}
