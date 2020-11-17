using System;
using System.Collections.Generic;
using System.Text;

namespace Project0.Library
{
    /// <summary>
    /// Provides data fields representing an order in
    /// the form of a list of Products
    /// </summary>
    public class Order
    {
        public Store TargetStore { get; set;  }
        public Customer Orderer { get; set; }
        public int OrderId;
        private static int _orderId = 1;

        public DateTime Time { get; set; }
        public List<Product> Selections { get; set; } = new List<Product>();

        public Order(Store targetStore, Customer orderer, List<Product> selections)
        {
            this.TargetStore = targetStore;
            this.Orderer = orderer;
            this.Selections = selections;
            this.OrderId = _orderId;
            this.Time = DateTime.Now;
            ++_orderId;
        }

        /// <summary>
        /// for stock orders, having no ordering customers
        /// </summary>
        public Order()
        {
            this.OrderId = _orderId;
            this.Time = DateTime.Now;
            ++_orderId;
        }

    }
}
