using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CongoData.StripeClient.Stripe.Models {
    public class SkuRequest {
        public string currency { get; set; }
        public SkuRequestInventory inventory { get; set; }
        public long price { get; set; }
        public string product { get; set; }
    }

    public class SkuRequestInventory {
        public string type { get; set; }
    }
}
