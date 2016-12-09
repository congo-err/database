using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CongoData.Client.Models {
    public class Order {
        public int OrderID { get; set; }
        public Customer Customer { get; set; }
        public Address Address { get; set; }
        public string StripeID { get; set; }
        public DateTime OrderDate { get; set; }
        public List<Product> Products { get; set; }
    }
}