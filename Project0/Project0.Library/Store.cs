using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Project0.Library
{
    class Store
    {
        public string Address { get;}
        public string ZipCode { get; }

        private ICollection<Product> _inventory;


        public Store( ICollection<Product> initialInventory, string address,string zip)
        {
            this.Address = address;
            this.ZipCode = zip;
            this._inventory = initialInventory;
        }

        public void AddItem(string itemName, int quantity)
        {
            //TODO: ensure duplicates are not added
            if( quantity >= 10000)
                throw new InvalidOperationException($"{quantity} is too large a quantity.");
            else if (quantity < 0)
                throw new InvalidOperationException($"{quantity} is not a valid quantity.");
            else 
            {
                _inventory.Add(new Product(itemName, quantity));
            }
        }

        public void RemoveItem(string itemName)
        {
            var product = _inventory.First(x => x.Name.Equals(itemName));
            _inventory.Remove(product);
        }
    }
}
