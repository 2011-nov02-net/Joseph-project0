using System;
using System.Collections.Generic;

#nullable disable

namespace Project0.Data
{
    public partial class StoreCustomer
    {
        public StoreCustomer()
        {
            Orders = new HashSet<Order>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
    }
}
