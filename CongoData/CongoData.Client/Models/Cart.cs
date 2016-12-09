using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CongoData.Client.Models {
    public class Cart {
        public Customer Customer { get; set; }
        public IEnumerable<Product> Products { get; set; }
    }
}