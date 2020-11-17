using System;
using System.Collections.Generic;

#nullable disable

namespace Project0.Data
{
    public partial class Product
    {
        public Product()
        {
            Items = new HashSet<Item>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }

        public virtual ICollection<Item> Items { get; set; }
    }
}
