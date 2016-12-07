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
    /// The Controller for access to Category entities.
    /// </summary>
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
    }
}
