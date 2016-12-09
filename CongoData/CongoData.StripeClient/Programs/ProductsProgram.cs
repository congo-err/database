using CongoData.DataAccess;
using CongoData.DataAccess.Concrete;
using CongoData.StripeClient.Stripe.Models;
using S = Stripe;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CongoData.StripeClient.Programs {
    public class ProductsProgram : IProgram {
        private bool generateFlag;

        public override void ExecuteFlag(string flag, string[] args) {
            if (flag == "-g") {
                GenerateFlag();
            }
        }

        public override int NumberOfArgs(string flag) {
            if (flag == "-g") {
                return 0;
            }

            return 0;
        }

        public override void ExecuteCommand() {
            //Stripe.StripeClient client = new Stripe.StripeClient();
            EfCongoRepository repository = new EfCongoRepository();
            List<Product> products = repository.ListProducts();

            if (generateFlag) {
                //Console.WriteLine("\tDeleting all Products and SKUs on Stripe...");
                //client.Reset();
                //Console.WriteLine("\tReset successful!\n");

                //foreach (Product product in products) {
                //    Console.WriteLine("\tCreating Stripe Product...");
                //    StripeModel stripeProduct = client.CreateProduct(product);

                //    if (stripeProduct.id == null) {
                //        client.WriteError(stripeProduct);
                //        continue;
                //    }

                //    Console.WriteLine(string.Format("\tProduct '{0}' creation successful!\n", stripeProduct.id));

                //    Console.WriteLine("\tCreating Stripe SKU...");
                //    StripeSku stripeSku = client.CreateSku(product, stripeProduct.id);

                //    if (stripeSku.id == null) {
                //        client.WriteError(stripeSku);
                //        continue;
                //    }

                //    Console.WriteLine(string.Format("\tSKU '{0}' creation successful!\n", stripeSku.id));

                //    Console.WriteLine("\tUpdating Product's StripeID in database...");
                //    repository.SetProductStripeID(product.ProductID, stripeSku.id);
                //    Console.WriteLine(string.Format("\tProduct '{0}' update successful!\n\n", product.ProductID));
                //}
                S.StripeChargeCreateOptions charge = new S.StripeChargeCreateOptions();
                charge.Amount = (int)Math.Floor(repository.GetProduct(1).Price * 100);
                charge.Currency = "usd";
                charge.SourceCard = new S.SourceCard {
                    Number = "4242424242424242",
                    ExpirationMonth = "3",
                    ExpirationYear = "2018",
                    Cvc = "123"
                };
                S.StripeChargeService chargeService = new S.StripeChargeService();
                S.StripeCharge sCharge = chargeService.Create(charge);
            }
        }

        private void GenerateFlag() {
            generateFlag = true;
        }
    }
}
