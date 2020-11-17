using System;
using System.Collections.Generic;

#nullable disable

namespace Project0.Data
{
    public partial class Order
    {
        public Order()
        {
            Items = new HashSet<Item>();
        }

        public int Id { get; set; }
        public int StoreId { get; set; }
        public int? CustomerId { get; set; }
        public DateTime Time { get; set; }

        public virtual StoreCustomer Customer { get; set; }
        public virtual Store Store { get; set; }
        public virtual ICollection<Item> Items { get; set; }
    }
}
