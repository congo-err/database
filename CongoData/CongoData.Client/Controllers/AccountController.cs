using CongoData.Client.Errors;
using CongoData.Client.Infrastructure;
using CongoData.DataAccess;
using CongoData.DataAccess.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;

namespace CongoData.Client.Controllers
{
    /// <summary>
    /// The Controller for access to Account entities.
    /// </summary>
    public class AccountController : ApiController
    {
        private readonly ICongoRepository repository;

        /// <summary>
        /// Create a new Account Controller.
        /// </summary>
        /// <param name="repository">The repository to get data from.</param>
        public AccountController(ICongoRepository repository) {
            this.repository = repository;
        }

        /// <summary>
        /// Get a single Account. If the Account doesn't exist, a 404 status code will be returned.
        /// </summary>
        /// <param name="id">The ID of the Account to get.</param>
        /// <returns>The Account object if successful or ErrorMessage object otherwise.</returns>
        [HttpGet]
        public HttpResponseMessage Get(int id) {
            Account account = repository.GetAccount(id);

            if (account == null) {
                return ErrorMessage.CreateResponse(Request, HttpStatusCode.NotFound, "Account with ID " + id + " was not found.");
            }

            return Request.CreateResponse(HttpStatusCode.OK, account, MediaTypes.Json);
        }
    }
}
