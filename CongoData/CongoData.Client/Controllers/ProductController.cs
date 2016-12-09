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

namespace CongoData.Client.Controllers {
    /// <summary>
    /// The Controller for access to Product entities.
    /// </summary>
    public class ProductController : ApiController {
        private readonly ICongoRepository repository;

        /// <summary>
        /// Create a new Product Controller.
        /// </summary>
        /// <param name="repository">The repository to get data from.</param>
        public ProductController(ICongoRepository repository) {
            this.repository = repository;
        }

        /// <summary>
        /// Get a single Order. If the Order doesn't exist, a 404 status code will be returned.
        /// </summary>
        /// <param name="id">The ID of the Order to get.</param>
        /// <returns>The Account object if successful or ErrorMessage object otherwise.</returns>
        [HttpGet]
        public HttpResponseMessage Get(int id) {
            Product product = repository.GetProduct(id);

            if (product == null) {
                return ErrorMessage.CreateResponse(Request, HttpStatusCode.NotFound, "The product with ID " + id + " was not found.");
            }

            return Request.CreateResponse(HttpStatusCode.OK, Mappers.Map(product), MediaTypes.Json);
        }

        /// <summary>
        /// Lists all of the Products in the database.
        /// </summary>
        /// <returns>A JSON object with an array of all Products.</returns>
        [HttpGet]
        public HttpResponseMessage List() {
            IEnumerable<Models.Product> products = Mappers.Map(repository.ListProducts());
            return Request.CreateResponse(HttpStatusCode.OK, products, MediaTypes.Json);
        }
    }
}
