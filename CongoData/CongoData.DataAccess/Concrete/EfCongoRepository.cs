﻿using CongoData.DataAccess.Abstract;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
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
        /// Get an Address.
        /// </summary>
        /// <param name="id">The ID of the Address.</param>
        /// <returns>The Address object or null if it was not found.</returns>
        public Address GetAddress(int id) {
            Address a = data.Addresses.Find(id);

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
        /// Add a Product to a customer's Cart.
        /// </summary>
        /// <param name="cartId">The ID of the customer's Cart.</param>
        /// <param name="productId">The ID of the Product to add.</param>
        /// <returns>Empty string if the addition was successful, an error message otherwise.</returns>
        public string AddProductToCart(int cartId, int productId) {
            Cart cart = GetCart(cartId);
            Product product = GetProduct(productId);

            if (cart == null) {
                return "Cart with ID " + cartId + " was not found.";
            }

            if (product == null) {
                return "Product with ID " + productId + " was not found.";
            }

            cart.Products.Add(product);

            if (data.SaveChanges() == 0) {
                return "Product is already within the cart.";
            }

            return string.Empty;
        }

        /// <summary>
        /// Remove a Product from a Cart.
        /// </summary>
        /// <param name="cartId">The ID of the customer's Cart.</param>
        /// <param name="productId">The ID of the Product to remove.</param>
        /// <returns>Empty string if the removal was successful, an error message otherwise.</returns>
        public string RemoveProductFromCart(int cartId, int productId) {
            Cart cart = GetCart(cartId);
            Product product = GetProduct(productId);

            if (cart == null) {
                return "Cart with ID " + cartId + " was not found.";
            }
            
            if (product == null) {
                return "Product with ID " + productId + " was not found.";
            }

            if (!cart.Products.Remove(product)) {
                return "Product is not in the cart.";
            }

            if (data.SaveChanges() == 0) {
                return "Unable to remove product from cart.";
            }

            return string.Empty;
        }

        /// <summary>
        /// Clear the Cart of all of its Products.
        /// </summary>
        /// <param name="cartId">The ID of the Cart.</param>
        /// <returns>Empty string if the removal was successful, an error message otherwise.</returns>
        public string ClearCart(int cartId) {
            Cart cart = GetCart(cartId);

            if (cart == null) {
                return "Cart with ID " + cartId + " was not found.";
            }

            cart.Products.Clear();

            if (data.SaveChanges() == 0) {
                return "Unable to clear cart.";
            }

            return string.Empty;
        }

        /// <summary>
        /// List all of the Categories.
        /// </summary>
        /// <returns>The List of Categories.</returns>
        public List<Category> ListCategories() {
            return data.Categories.Where(c => c.Active).ToList();
        }

        /// <summary>
        /// List all of the Orders.
        /// </summary>
        /// <returns>The List of Orders.</returns>
        public List<Order> ListOrders() {
            return data.Orders.Where(o => o.Active).ToList();
        }

        /// <summary>
        /// List all of the Orders from a Customer.
        /// </summary>
        /// <param name="customerId">The ID of the Customer.</param>
        /// <returns>The List of Orders.</returns>
        public List<Order> ListOrdersFromCustomer(int customerId) {
            return data.Orders.Where(o => o.Active && o.CustomerID == customerId).ToList();
        }

        /// <summary>
        /// Get a Customer.
        /// </summary>
        /// <param name="id">The ID of the Customer.</param>
        /// <returns>The Customer object or null if it was not found.</returns>
        public Order GetOrder(int id) {
            Order o = data.Orders.Find(id);

            if (o != null && o.Active) {
                return o;
            }

            return null;
        }

        /// <summary>
        /// Create an Order and save it to the database.
        /// </summary>
        /// <param name="order">The Order object to save.</param>
        /// <returns>String containing the new order id if the addition was successful, and error message otherwise.</returns>
        public string CreateOrder(int customerID, int addressID, string stripeID, List<int> productIDs) {
            Customer customer = GetCustomer(customerID);
            Address address = GetAddress(addressID);
            List<Product> products = new List<Product>();

            if (customer == null) {
                return "Customer with ID " + customerID + " was not found.";
            }

            if (address == null) {
                return "Address with ID " + addressID + " was not found.";
            }

            foreach (int productID in productIDs) {
                Product product = GetProduct(productID);

                if (product == null) {
                    return "Product with ID " + productID + " was not found.";
                }

                products.Add(product);
            }

            data.Orders.Add(new Order {
                Address = address,
                Customer = customer,
                StripeID = stripeID,
                Products = products,
                CreatedDate = DateTime.Now,
                UpdatedDate = DateTime.Now,
                Active = true
            });

            if (data.SaveChanges() == 0) {
                return "Order could not be created.";
            }

            return data.Orders.ToList().Count.ToString();
        }

        /// <summary>
        /// List all of the Products in a Category.
        /// </summary>
        /// <param name="categoryId">The ID of the Category.</param>
        /// <returns>The List of Products.</returns>
        public List<Product> ListProductsInCategory(int categoryId) {
            return data.Products.Where(p => p.Active && p.CategoryID == categoryId).ToList();
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
        
        /// <summary>
        /// Get a Product.
        /// </summary>
        /// <param name="id">The ID of the Product.</param>
        /// <returns>The Product object or null if it was not found.</returns>
        public Product GetProduct(int id) {
            Product p = data.Products.Find(id);

            if (p != null && p.Active) {
                return p;
            }

            return null;
        }

        /// <summary>
        /// List all of the Products.
        /// </summary>
        /// <returns></returns>
        public List<Product> ListProducts() {
            return data.Products.Where(p => p.Active).ToList();
        }

        /// <summary>
        /// Updates a Product's StripeID.
        /// </summary>
        /// <param name="productId">The ID of the Product.</param>
        /// <param name="stripeId">The ID of the SKU on Stripe.</param>
        /// <returns>True if the change was successful, false otherwise.</returns>
        public bool SetProductStripeID(int productId, string stripeId) {
            Product product = GetProduct(productId);

            if (product == null) {
                return false;
            }

            DbEntityEntry<Product> productEntry = data.Entry(product);
            product.StripeID = stripeId;
            productEntry.State = System.Data.Entity.EntityState.Modified;

            return data.SaveChanges() > 0;
        }
    }
}
