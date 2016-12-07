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

        /// <summary>
        /// Add a Product to a Cart.
        /// </summary>
        /// <param name="cartProduct">The IDs of the Cart and Product.</param>
        /// <returns>OK and success of true if the addition was successful, OK and an erorr message otherwise.</returns>
        [HttpPost]
        public HttpResponseMessage Post([FromBody] Models.CartProduct cartProduct) {
            string errorMessage = repository.AddProductToCart(cartProduct.CartID, cartProduct.ProductID);

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

        /// <summary>
        /// Remove a Product from a Cart.
        /// </summary>
        /// <param name="cartProduct">The IDs of the Cart and Product.</param>
        /// <returns>OK and success of true if the addition was successful, OK and an erorr message otherwise.</returns>
        [HttpDelete]
        public HttpResponseMessage Delete([FromBody] Models.CartProduct cartProduct) {
            string errorMessage = repository.RemoveProductFromCart(cartProduct.CartID, cartProduct.ProductID);

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
