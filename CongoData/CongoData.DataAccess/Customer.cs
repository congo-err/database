//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CongoData.DataAccess
{
    using System;
    using System.Collections.Generic;
    
    public partial class Customer
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Customer()
        {
            this.Orders = new HashSet<Order>();
        }
    
        public int CustomerID { get; set; }
        public string Name { get; set; }
        public int AccountID { get; set; }
        public int AddressID { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public System.DateTime UpdatedDate { get; set; }
        public bool Active { get; set; }
    
        public virtual Address Address { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Order> Orders { get; set; }
        public virtual Account Account { get; set; }
    }
}
