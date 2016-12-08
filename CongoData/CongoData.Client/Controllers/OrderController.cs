using CongoData.Client.Infrastructure;
using CongoData.DataAccess;
using CongoData.DataAccess.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace CongoData.Client.Controllers {
    /// <summary>
    /// The Controller for access to Order entities.
    /// </summary>
    public class OrderController : ApiController {
        private readonly ICongoRepository repository;

        /// <summary>
        /// Create a new Order Controller.
        /// </summary>
        /// <param name="repository">The repository to get data from.</param>
        public OrderController(ICongoRepository repository) {
            this.repository = repository;
        }

        /// <summary>
        /// Lists all of the Orders.
        /// </summary>
        /// <returns>A JSON object with an array of all Orders.</returns>
        [HttpGet]
        public HttpResponseMessage List() {
            List<Models.Order> orders = Mappers.Map(repository.ListOrders());
            return Request.CreateResponse(HttpStatusCode.OK, orders, MediaTypes.Json);
        }

        /// <summary>
        /// Lists all of the Orders made by a specific Customer in the database.
        /// </summary>
        /// <returns>A JSON object with an array of all Orders.</returns>
        [HttpGet]
        public HttpResponseMessage List(int id) {
            List<Models.Order> orders = Mappers.Map(repository.ListOrdersFromCustomer(id));
            return Request.CreateResponse(HttpStatusCode.OK, orders, MediaTypes.Json);
        }

        /// <summary>
        /// Create an Order.
        /// </summary>
        /// <param name="order">The IDs for the customer, address, stripe order, and products.</param>
        /// <returns>OK and success of true if the creation was successful, OK and an erorr message otherwise.</returns>
        [HttpPost]
        public HttpResponseMessage Post([FromBody] Models.OrderRequest order) {
            string errorMessage = repository.CreateOrder(order.CustomerID, order.AddressID, order.StripeID, order.ProductIDs);

            if (errorMessage == string.Empty) {
                return Request.CreateResponse(HttpStatusCode.OK, new Models.PostResponseBody {
                    Success = true
                }, MediaTypes.Json);
            }

            return Request.CreateResponse(HttpStatusCode.OK, new Models.PostResponseBody {
                Success = false,
                Message = errorMessage
            }, MediaTypes.Json);
        }
    }
}