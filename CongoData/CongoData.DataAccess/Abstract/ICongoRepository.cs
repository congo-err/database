using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CongoData.DataAccess.Abstract {
    public interface ICongoRepository {
        // Account
        Account GetAccount(int id);
        Account GetAccountByUsername(string username);

        // Cart
        Cart GetCart(int id);

        // Category
        List<Category> ListCategories();

        // Customer
        Customer GetCustomer(int id);
    }
}
