using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Formatting;
using System.Web.Http;

namespace CongoData.Client
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            // JSON Serializer
            JsonMediaTypeFormatter json = config.Formatters.JsonFormatter;
            json.SerializerSettings.Formatting = Newtonsoft.Json.Formatting.Indented;

            // CORS
            config.EnableCors();

            // Account
            config.Routes.MapHttpRoute(
                name: "",
                routeTemplate: "account/try-login",
                defaults: new { Controller = "Account", Action = "TryLogin" }
            );

            // Carts
            config.Routes.MapHttpRoute(
                name: "",
                routeTemplate: "cart/{cartID}/{productID}",
                defaults: new { Controller = "Cart", Action = "Delete" }
            );

            // Categories
            config.Routes.MapHttpRoute(
                name: "",
                routeTemplate: "category",
                defaults: new { controller = "Category", Action = "List" }
            );

            // Orders
            config.Routes.MapHttpRoute(
                name: "",
                routeTemplate: "order/list/{id}",
                defaults: new { controller = "Order", Action = "List", id = RouteParameter.Optional }
            );

            // Products
            config.Routes.MapHttpRoute(
                name: "",
                routeTemplate: "product",
                defaults: new { controller = "Product", Action = "List" }
            );

            // Default
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
