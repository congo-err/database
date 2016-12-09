using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CongoData.StripeClient.Stripe.Models {
    public class StripeModel {
        public string id { get; set; }
        public long created { get; set; }
        public long updated { get; set; }
        public bool active { get; set; }

        // Error
        public string type { get; set; }
        public string message { get; set; }
    }
}
