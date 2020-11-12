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
        public string Id { get; }
        private static int IdSeed = 1;
        private List<Order> OrderHistory;

        //TODO: add default (preferred) store
        public Customer(string name)
        {
            this.Name = name;
            this.Id = IdSeed.ToString();
            ++IdSeed;
        }
        /// <summary>
        /// adds an order to the customer's history
        /// </summary>
        public void AddToOrderHistory(Order order)
        {
            //may want to only track OrderIds
            OrderHistory.Add(order);
        }
    }
}
