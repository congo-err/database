using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CongoData.Client.Models {
    public class Customer {
        public int CustomerID { get; set; }
        public string Name { get; set; }
        public Account Account { get; set; }
        public Address Address { get; set; }
    }
}