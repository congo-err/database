using CongoData.Client.Infrastructure;
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
        /// Lists all of the Orders made by a specific Customer in the database.
        /// </summary>
        /// <returns>A JSON object with an array of all Orders.</returns>
        [HttpGet]
        public HttpResponseMessage List(int id) {
            List<Models.Order> orders = Mappers.Map(repository.ListOrdersFromCustomer(id));
            return Request.CreateResponse(HttpStatusCode.OK, orders, MediaTypes.Json);
        }
    }
}