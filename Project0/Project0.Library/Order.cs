using System;
using System.Collections.Generic;
using System.Text;

namespace Project0.Library
{
    public class Order
    {
       // public Store TargetStore { get; set;  }
        public Customer Orderer { get; set; }
        public string OrderId;
        private static int _orderId = 1;
        public List<Product> Selections;

        public Order(/*Store targetStore,*/ Customer orderer, List<Product> selections)
        {
           // this.TargetStore = targetStore;
            this.Orderer = orderer;
            this.Selections = selections;
            this.OrderId = _orderId.ToString();
            ++_orderId;
        }

    }
}
