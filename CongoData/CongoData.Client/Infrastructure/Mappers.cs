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
    }
}