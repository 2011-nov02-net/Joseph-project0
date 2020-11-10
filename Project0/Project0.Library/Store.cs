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
            if (!this._customers.Contains(customer))
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
            else if (quantity < 1)
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
        public List<bool> FillOrder(Order order)
        {
            //add new customer to customer list
            AddCustomer(order.Orderer);
            List<bool> orderResults = new List<bool>();

            foreach (Product inventoryItem in this._inventory)
            {
                bool carryItem = order.Selections.Exists(x => x.Name == inventoryItem.Name);
            
                //TODO: add reason for failure to fill
                if ( carryItem && inventoryItem.InStock)
                {
                    Product orderSelection = order.Selections.Find(x => x.Name == inventoryItem.Name);
                    if (orderSelection.Quantity > inventoryItem.Quantity)
                        orderResults.Add(false);
                    else
                    {
                        inventoryItem.Quantity -= orderSelection.Quantity;
                        //TODO: add check for InStock, after filling order
                        orderResults.Add(true);
                    }
                }
                else
                {
                    orderResults.Add(false);
                }
            }
            return orderResults;
        }
        public void Restock(Order restockOrder)
        {
            foreach (Product inventoryItem in this._inventory)
            {
                bool carryItem = restockOrder.Selections.Exists(x => x.Name == inventoryItem.Name);
                if (carryItem)
                {
                    Product orderSelection = restockOrder.Selections.Find(x => x.Name == inventoryItem.Name);
                    inventoryItem.Quantity += orderSelection.Quantity;
                }
                else
                {
                    Product orderSelection = restockOrder.Selections.Find(x => x.Name == inventoryItem.Name);
                    AddItem(orderSelection.Name, orderSelection.Quantity);
                }
            }
        }
    }
}