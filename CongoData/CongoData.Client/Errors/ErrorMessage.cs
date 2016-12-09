using CongoData.Client.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;

namespace CongoData.Client.Errors {
    public class ErrorMessage {
        public static HttpResponseMessage CreateResponse(HttpRequestMessage request, HttpStatusCode status, string message) {
            return request.CreateResponse(status, new ErrorMessage {
                Message = message
            }, MediaTypes.Json);
        }

        public string Message { get; set; }
    }
}