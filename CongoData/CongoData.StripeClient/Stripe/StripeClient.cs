using CongoData.DataAccess;
using CongoData.StripeClient.Stripe.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace CongoData.StripeClient.Stripe {
    public class StripeClient {
        private const string BASE_URL = "https://api.stripe.com/v1/";
        private readonly HttpClient client;

        public StripeClient() {
            string token = Convert.ToBase64String(Encoding.UTF8.GetBytes(ConfigurationManager.AppSettings["StripeApiKey"] + ":"));

            client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", token);
        }

        public StripeSku GetSku(string id) {
            return Read<StripeSku>("skus/" + id);
        }

        public StripeModel GetProduct(string id) {
            return Read<StripeModel>("products/" + id);
        }

        public StripeSku CreateSku(Product product, string productId) {
            return Create<SkuRequest, StripeSku>("", new SkuRequest {
                currency = "usd",
                inventory = new SkuRequestInventory { type = "infinite" },
                price = (long) Math.Floor(product.Price),
                product = productId
            });
        }

        public StripeModel CreateProduct(Product product) {
            return Create<ProductRequest, StripeModel>("products", new ProductRequest {
                name = product.Name,
                description = product.Description
            });
        }

        public void Reset() {
            List<StripeSku> skus = Read<List<StripeSku>>(BASE_URL + "skus");

            foreach (StripeSku sku in skus) {
                Delete("skus/" + sku.id);
                Delete("products/" + sku.product);
            }
        }

        public void WriteError(StripeModel model) {
            Console.WriteLine(string.Format("\tError: {0}\n\t\t{1}\n", model.type, model.message));
        }

        private TResponse Create<TRequest, TResponse>(string url, TRequest o) where TRequest : class where TResponse : class, new() {
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(TRequest));
            MemoryStream stream = new MemoryStream();
            serializer.WriteObject(stream, o);
            stream.Position = 0;
            StreamReader reader = new StreamReader(stream);
            StringContent content = new StringContent(reader.ReadToEnd(), Encoding.UTF8, "application/json");

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, BASE_URL + url);
            //request.RequestUri = new Uri(BASE_URL + url);
            //request.Method = HttpMethod.Post;
            request.Headers.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.UTF8.GetBytes(ConfigurationManager.AppSettings["StripeApiKey"] + ":")));
            request.Headers.Host = "api.stripe.com";
            request.Headers.CacheControl = new CacheControlHeaderValue { NoCache = true };
            //request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", ConfigurationManager.AppSettings["StripeApiKey"]);
            request.Content = content;

            //HttpResponseMessage response = client.PostAsync(BASE_URL + url, content).Result;
            HttpResponseMessage response = client.SendAsync(request).Result;

            if (response.IsSuccessStatusCode) {
                JavaScriptSerializer deserializer = new JavaScriptSerializer();
                return deserializer.Deserialize<TResponse>(response.Content.ReadAsStringAsync().Result);
            }

            return new TResponse();
        }

        private T Read<T>(string url) where T : class, new() {
            HttpResponseMessage response = client.GetAsync(BASE_URL + url).Result;

            if (response.IsSuccessStatusCode) {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                return serializer.Deserialize<T>(response.Content.ReadAsStringAsync().Result);
            }

            return new T();
        }

        private bool Delete(string url) {
            HttpResponseMessage response = client.DeleteAsync(BASE_URL + url).Result;

            if (response.IsSuccessStatusCode) {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                return serializer.Deserialize<StripeDelete>(response.Content.ReadAsStringAsync().Result).deleted;
            }

            return false;
        }
    }
}
