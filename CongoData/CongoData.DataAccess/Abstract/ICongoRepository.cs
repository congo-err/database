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
        string AddProductToCart(int cartId, int productId);
        string RemoveProductFromCart(int cartId, int productId);

        // Category
        List<Category> ListCategories();
        List<Product> ListProductsInCategory(int categoryId);

        // Customer
        Customer GetCustomer(int id);

        // Order
        List<Order> ListOrders();
        List<Order> ListOrdersFromCustomer(int customerId);

        // Product
        Product GetProduct(int id);
        List<Product> ListProducts();
    }
}
