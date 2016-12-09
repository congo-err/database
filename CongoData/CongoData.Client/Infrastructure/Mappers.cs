using CongoData.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CongoData.Client.Infrastructure {
    /// <summary>
    /// The class for Mapping the Entity Framework entities to the API models.
    /// </summary>
    public static class Mappers {
        /// <summary>
        /// Map an Account.
        /// </summary>
        /// <param name="account">The Entity Framework Account.</param>
        /// <returns>The API Account model.</returns>
        public static Models.Account Map(Account account) {
            return new Models.Account {
                AccountID = account.AccountID,
                Role = account.Role,
                Username = account.Username
            };
        }

        /// <summary>
        /// Map a List of Accounts.
        /// </summary>
        /// <param name="list">The List of Entity Framework Accounts.</param>
        /// <returns>The List of API Account models.</returns>
        public static List<Models.Account> Map(List<Account> list) {
            List<Models.Account> outList = new List<Models.Account>();

            foreach (Account item in list) {
                outList.Add(Map(item));
            }

            return outList;
        }

        /// <summary>
        /// Map an Address.
        /// </summary>
        /// <param name="address">The Entity Framework Address.</param>
        /// <returns>The API Address model.</returns>
        public static Models.Address Map(Address address) {
            return new Models.Address {
                AddressID = address.AddressID,
                City = address.City,
                State = address.State,
                Street = address.Street,
                Zip = address.Zip
            };
        }

        /// <summary>
        /// Map a List of Addresses.
        /// </summary>
        /// <param name="list">The List of Entity Framework Addresses.</param>
        /// <returns>The List of API Address models.</returns>
        public static List<Models.Address> Map(List<Address> list) {
            List<Models.Address> outList = new List<Models.Address>();

            foreach (Address item in list) {
                outList.Add(Map(item));
            }

            return outList;
        }

        /// <summary>
        /// Map a Cart.
        /// </summary>
        /// <param name="cart">The Entity Framework Cart.</param>
        /// <returns>The API Cart model.</returns>
        public static Models.Cart Map(Cart cart, Customer customer) {
            Models.Cart c = new Models.Cart {
                Customer = Map(customer)
            };

            List<Models.Product> products = new List<Models.Product>();
            foreach (Product product in cart.Products) {
                products.Add(Map(product));
            }

            c.Products = products;
            return c;
        }

        /// <summary>
        /// Map a Customer.
        /// </summary>
        /// <param name="customer">The Entity Framework Customer.</param>
        /// <returns>The API Customer model.</returns>
        public static Models.Customer Map(Customer customer) {
            return new Models.Customer {
                Account = Map(customer.Account),
                Address = Map(customer.Address),
                CustomerID = customer.CustomerID,
                Name = customer.Name
            };
        }

        /// <summary>
        /// Map a List of Customers.
        /// </summary>
        /// <param name="list">The List of Entity Framework Customers.</param>
        /// <returns>The List of API Customer models.</returns>
        public static List<Models.Customer> Map(List<Customer> list) {
            List<Models.Customer> outList = new List<Models.Customer>();

            foreach (Customer item in list) {
                outList.Add(Map(item));
            }

            return outList;
        }

        /// <summary>
        /// Map an Category.
        /// </summary>
        /// <param name="category">The Entity Framework Category.</param>
        /// <returns>The API Category model.</returns>
        public static Models.Category Map(Category category) {
            return new Models.Category {
                CategoryID = category.CategoryID,
                Name = category.Name
            };
        }

        /// <summary>
        /// Map a List of Categories.
        /// </summary>
        /// <param name="list">The List of Entity Framework Categories.</param>
        /// <returns>The List of API Category models.</returns>
        public static List<Models.Category> Map(List<Category> list) {
            List<Models.Category> outList = new List<Models.Category>();

            foreach (Category item in list) {
                outList.Add(Map(item));
            }

            return outList;
        }

        /// <summary>
        /// Map an Order.
        /// </summary>
        /// <param name="order">The Entity Framework Order.</param>
        /// <returns>The API Order model.</returns>
        public static Models.Order Map(Order order) {
            return new Models.Order {
                Address = Map(order.Address),
                Customer = Map(order.Customer),
                OrderDate = order.CreatedDate,
                OrderID = order.OrderID,
                Products = Map(order.Products.ToList()),
                StripeID = order.StripeID
            };
        }

        /// <summary>
        /// Map a List of Orders.
        /// </summary>
        /// <param name="list">The List of Entity Framework Orders.</param>
        /// <returns>The List of API Order models.</returns>
        public static List<Models.Order> Map(List<Order> list) {
            List<Models.Order> outList = new List<Models.Order>();

            foreach (Order item in list) {
                outList.Add(Map(item));
            }

            return outList;
        }

        /// <summary>
        /// Map an Product.
        /// </summary>
        /// <param name="product">The Entity Framework Product.</param>
        /// <returns>The API Product model.</returns>
        public static Models.Product Map(Product product) {
            return new Models.Product {
                Category = Map(product.Category),
                Description = product.Description,
                ImagePath = product.ImagePath,
                Name = product.Name,
                Price = product.Price,
                ProductID = product.ProductID
            };
        }

        /// <summary>
        /// Map a List of Products.
        /// </summary>
        /// <param name="list">The List of Entity Framework Products.</param>
        /// <returns>The List of API Product models.</returns>
        public static List<Models.Product> Map(List<Product> list) {
            List<Models.Product> outList = new List<Models.Product>();

            foreach (Product item in list) {
                outList.Add(Map(item));
            }

            return outList;
        }
    }
}