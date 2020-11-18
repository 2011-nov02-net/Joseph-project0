using System;
using System.Collections.Generic;
using System.Text;

namespace Project0.Library
{
    /// <summary>
    /// used to stock a Store
    /// </summary>
    public class RestockOrder : IOrder
    {
        public Store TargetStore { get; set; }
        public int OrderId { get; set; }
        private static int _orderId = 1;

        public DateTime Time { get; set; }
        public List<Product> Selections { get; set; } = new List<Product>();

        public RestockOrder()
        {
            this.OrderId = _orderId;
            this.Time = DateTime.Now;
            ++_orderId;
        }
    }
}
