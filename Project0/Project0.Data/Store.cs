using System;
using System.Collections.Generic;

#nullable disable

namespace Project0.Data
{
    public partial class Store
    {
        public Store()
        {
            Items = new HashSet<Item>();
            Orders = new HashSet<Order>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }

        public virtual ICollection<Item> Items { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }
}
