using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CongoData.Client.Models {
    public class OrderResponse {
        public bool Success { get; set; }
        public string Message { get; set; }
        public Order Order { get; set; }
    }
}