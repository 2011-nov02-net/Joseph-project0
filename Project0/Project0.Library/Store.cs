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
        public string Name { get; }

        public int Id { get; set; }
        private static int _Id = 1;

        public List<Product> Inventory { get; set; }
        public List<Customer> Customers { get; set; }

        public Store( List<Product> initialInventory, string address,string name)
        {
            this.Address = address;
            this.Name = name;
            this.Inventory = initialInventory;
            this.Customers = new List<Customer>();
            this.Id = _Id;
            ++_Id;

        }
        public Store( string address, string name)
        {
            this.Address = address;
            this.Name = name;
            this.Inventory = new List<Product>();
            this.Customers = new List<Customer>();
            this.Id = _Id;
            ++_Id;

        }
        /// <summary>
        /// Adds a customer to the store's customer list
        /// </summary>
        public void AddCustomer(Customer customer)
        {
            if (!this.Customers.Contains(customer))
                Customers.Add(customer);
        }

        /// <summary>
        /// removes a customer from the store's customer list
        /// </summary>
        public void RemoveCustomer(Customer customer)
        {
            if (this.Customers.Contains(customer))
                Customers.Remove(customer);
        }
        /// <summary>
        /// Adds an item to the store's inventory list
        /// </summary>
        public void AddItem(string itemName, int quantity)
        {
            //TODO: ensure duplicates are not added
            if( quantity >= 1000)
                throw new InvalidOperationException($"{quantity} is too large a quantity.");
            else if (quantity < 1)
                throw new InvalidOperationException($"{quantity} is not a valid quantity.");
            else 
            {
                Inventory.Add(new Product(itemName, quantity) { InStock = true });
            }
        }

        /// <summary>
        /// removes an item from the inventory list
        /// </summary>
        public void RemoveItem(string itemName)
        {
            var product = Inventory.First(x => x.Name.Equals(itemName));
            Inventory.Remove(product);
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
            

            foreach(Product orderItem in order.Selections)
            {
                bool carryItem = this.Inventory.Exists(x => x.Name == orderItem.Name);
                Product inventoryItem = this.Inventory.FirstOrDefault(x => x.Name == orderItem.Name);
                bool inStock = inventoryItem.Quantity > 0;
                if (carryItem && inStock)
                {
                    if (orderItem.Quantity > inventoryItem.Quantity)
                        orderResults.Add(false);
                    else
                    {
                        inventoryItem.Quantity -= orderItem.Quantity;
                        //TODO: add check for InStock, after filling order
                        orderResults.Add(true);
                    }
                }
                else
                {
                    orderResults.Add(false);
                }
            }
            order.Orderer.AddToOrderHistory(order);
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
                bool carryItem = this.Inventory.Exists(x => x.Name == orderItem.Name);
                //if item is carried by the store, restock it with the order quantity
                //else add item to the list of items carried by the store
                if (carryItem)
                {
                    Product restockSelection = this.Inventory.Find(x => x.Name == orderItem.Name);
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