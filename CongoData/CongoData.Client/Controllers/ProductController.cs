using CongoData.Client.Infrastructure;
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
