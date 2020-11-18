using System;
using System.Collections.Generic;

namespace Project0.Library
{
    /// <summary>
    /// Provides data fields representing a customer
    /// </summary>
    public class Customer
    {
        public string Name { get; }
        public int Id { get; set; }
        private static int IdSeed = 1;
        public List<Order> OrderHistory {get;set;}

        //TODO: add default (preferred) store
        public Customer(string name)
        {
            this.OrderHistory = new List<Order>();
            this.Name = name;
            this.Id = IdSeed;
            ++IdSeed;
        }
        /// <summary>
        /// adds an order to the customer's history
        /// </summary>
        public void AddToOrderHistory(Order order)
        {
            //may want to only track OrderIds
            if (order != null)
                this.OrderHistory.Add(order);
        }
    }
}
