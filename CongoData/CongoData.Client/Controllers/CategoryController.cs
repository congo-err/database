using CongoData.Client.Infrastructure;
using CongoData.DataAccess.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace CongoData.Client.Controllers {
    /// <summary>
    /// The Controller for access to Category entities.
    /// </summary>
    [EnableCors("http://http://ec2-34-192-6-56.compute-1.amazonaws.com", "*", "*")]
    public class CategoryController : ApiController
    {
        private readonly ICongoRepository repository;
        
        /// <summary>
        /// Create a new Category Controller.
        /// </summary>
        /// <param name="repository">The repository to get data from.</param>
        public CategoryController(ICongoRepository repository) {
            this.repository = repository;
        }

        /// <summary>
        /// Lists all of the Categories in the database.
        /// </summary>
        /// <returns>A JSON object with an array of all Catgories.</returns>
        [HttpGet]
        public HttpResponseMessage List() {
            IEnumerable<Models.Category> categories = Mappers.Map(repository.ListCategories());
            return Request.CreateResponse(HttpStatusCode.OK, categories, MediaTypes.Json);
        }

        /// <summary>
        /// Lists all of the Products in the database with a given Category.
        /// </summary>
        /// <param name="id">The ID of the Category.</param>
        /// <returns>A JSON object with an array of all Products.</returns>
        [HttpGet]
        public HttpResponseMessage Get(int id) {
            IEnumerable<Models.Product> products = Mappers.Map(repository.ListProductsInCategory(id));
            return Request.CreateResponse(HttpStatusCode.OK, products, MediaTypes.Json);
        }
    }
}
