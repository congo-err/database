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

        // Address
        Address GetAddress(int id);

        // Cart
        Cart GetCart(int id);
        string AddProductToCart(int cartId, int productId);
        string RemoveProductFromCart(int cartId, int productId);
        string ClearCart(int id);

        // Category
        List<Category> ListCategories();
        List<Product> ListProductsInCategory(int categoryId);

        // Customer
        Customer GetCustomer(int id);

        // Order
        List<Order> ListOrders();
        List<Order> ListOrdersFromCustomer(int customerId);
        Order GetOrder(int id);
        string CreateOrder(int customerID, int addressID, string stripeID, List<int> productIDs);

        // Product
        Product GetProduct(int id);
        List<Product> ListProducts();
        bool SetProductStripeID(int productId, string stripeId);
    }
}
