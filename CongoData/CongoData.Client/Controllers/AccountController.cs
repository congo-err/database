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

            return Request.CreateResponse(HttpStatusCode.OK, Mappers.Map(account), MediaTypes.Json);
        }

        /// <summary>
        /// Try to login a user.
        /// </summary>
        /// <param name="username">The username of the user.</param>
        /// <param name="password">The password of the user.</param>
        /// <returns>If successful, the Account object. Otherwise, a message and boolean that contains the results of the login.</returns>
        [HttpPost]
        public HttpResponseMessage TryLogin([FromBody] Models.UsernamePasswordPair pair) {
            Account account = repository.GetAccountByUsername(pair.Username);

            if (account == null) {
                return Request.CreateResponse(HttpStatusCode.OK, new {
                    Success = false,
                    Message = "No user with username " + pair.Username + " was found."
                }, MediaTypes.Json);
            }

            if (account.Password != pair.Password) {
                return Request.CreateResponse(HttpStatusCode.OK, new {
                    Success = false,
                    Message = "The username/password combination was incorrect."
                }, MediaTypes.Json);
            }

            return Request.CreateResponse(HttpStatusCode.OK, new {
                Success = true,
                Account = Mappers.Map(account)
            }, MediaTypes.Json);
        }
    }
}
