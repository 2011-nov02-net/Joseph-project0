using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Project0.Library
{
    public class Store
    {
        public string Address { get;}
        public string ZipCode { get; }

        private List<Product> _inventory;
        private List<Customer> _customers;

        public Store( List<Product> initialInventory, string address,string zip)
        {
            this.Address = address;
            this.ZipCode = zip;
            this._inventory = initialInventory;

        }

        public void AddCustomer(Customer customer)
        {
            _customers.Add(customer);
        }

        public void RemoveCustomer(Customer customer)
        {
            _customers.Remove(customer);
        }
        public void PrintInventory()
        {
            //TODO
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
                _inventory.Add(new Product(itemName, quantity) { InStock = true });
            }
        }

        public void RemoveItem(string itemName)
        {
            var product = _inventory.First(x => x.Name.Equals(itemName));
            _inventory.Remove(product);
        }
        //returns true if order can be met
        public bool FillOrder(Order order)
        {
            //add new customer to customer list
            if (!this._customers.Contains(order.Orderer))
                _customers.Add(order.Orderer);

            foreach(Product item in this._inventory)
            {
                //TODO: add message for failure to fill, and finish logic for finding 
                if (order.Selections.Contains(itemName) && item.InStock)
                {
                    
                   
                }
                else
                {
                    return false;
                }
            }
            return true;
        }
        public void Restock(Order restockOrder)
        {

        }
    }
}
