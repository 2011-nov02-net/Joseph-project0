using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Project0.Library
{
    /// <summary>
    /// Store has feilds to track it's location, customer list(customers) and inventory list(product)
    /// </summary>
    /// <remarks>
    /// Store handles the majority of the business logic, with the other classes primarily used to 
    /// provide data to be handled by store methods
    /// </remarks>
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

        /// <summary>
        /// Adds a customer to the store's customer list
        /// </summary>
        public void AddCustomer(Customer customer)
        {
            if (!this._customers.Contains(customer))
                _customers.Add(customer);
        }

        /// <summary>
        /// removes a customer from the store's customer list
        /// </summary>
        public void RemoveCustomer(Customer customer)
        {
            _customers.Remove(customer);
        }
        /// <summary>
        /// outputs the list of product names, with their quantity at the targeted store
        /// </summary>
        public void PrintInventory()
        {
            //may be better to move to Program.cs

        }

        /// <summary>
        /// Adds an item to the store's inventory list
        /// </summary>
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

        /// <summary>
        /// removes an item from the inventory list
        /// </summary>
        public void RemoveItem(string itemName)
        {
            var product = _inventory.First(x => x.Name.Equals(itemName));
            _inventory.Remove(product);
        }

        /// <summary>
        ///returns a list of bools, each item indicates whether the order was met 
        ///lowers product quantity to reflect filled order, but does not remove 
        ///products from inventory list
        /// </summary>
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
        /// <summary>
        /// if the product is in the inventory list, adds quantity
        /// if not, product is added to inventory with initial quantity
        /// from restock
        /// </summary>
        public void Restock(Order restockOrder)
        {
            foreach (Product orderItem in restockOrder.Selections)
            {
                bool carryItem = this._inventory.Exists(x => x.Name == orderItem.Name);
                //if item is carried by the store, restock it with the order quantity
                //else add item to the list of items carried by the store
                if (carryItem)
                {
                    Product restockSelection = this._inventory.Find(x => x.Name == orderItem.Name);
                    restockSelection.Quantity += orderItem.Quantity;
                }
                else
                {
                    AddItem(orderItem.Name, orderItem.Quantity);
                }
            }

        }
    }
}