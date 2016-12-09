using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CongoData.Client.Models {
    public class OrderRequest {
        public int CustomerID { get; set; }
        public int AddressID { get; set; }
        public string StripeID { get; set; }
        public List<int> ProductIDs { get; set; }
    }
}