using System;
using System.Collections.Generic;

namespace Project0.Library
{
    public class Customer
    {
        public string Name { get; }
        public string Id { get; }
        private static int IdSeed = 1;
        public List<Order> OrderHistory;

        //TODO: add default (preferred) store
        public Customer(string name)
        {
            this.Name = name;
            this.Id = IdSeed.ToString();
            ++IdSeed;
        }
    }
}
