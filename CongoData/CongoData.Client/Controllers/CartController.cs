using CongoData.Client.Errors;
using CongoData.Client.Infrastructure;
using CongoData.DataAccess;
using CongoData.DataAccess.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace CongoData.Client.Controllers
{
    /// <summary>
    /// The Controller to access Cart Entities.
    /// </summary>
    public class CartController : ApiController
    {
        private ICongoRepository repository;

        /// <summary>
        /// Create a new Cart Controller.
        /// </summary>
        /// <param name="repository">The repository to get data from.</param>
        public CartController(ICongoRepository repository) {
            this.repository = repository;
        }

        /// <summary>
        /// Get a Cart. If it doesn't exist, a 404 status code will be returned.
        /// </summary>
        /// <param name="id">The ID of the Cart.</param>
        /// <returns>The Cart object if it was successful or an ErrorMessage otherwise.</returns>
        [HttpGet]
        public HttpResponseMessage Get(int id) {
            Cart cart = repository.GetCart(id);
            Customer customer = repository.GetCustomer(id);

            if (cart == null || customer == null) {
                return ErrorMessage.CreateResponse(Request, HttpStatusCode.NotFound, "Cart with ID " + id + " was not found.");
            }

            return Request.CreateResponse(HttpStatusCode.OK, Mappers.Map(cart, customer), MediaTypes.Json);
        }
    }
}
