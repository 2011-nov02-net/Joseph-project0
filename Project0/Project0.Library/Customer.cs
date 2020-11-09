using System;

namespace Project0.Library
{
    public class Customer
    {
        public string Name { get; }
        public string Id { get; }
        private static int IdSeed = 1;
        //TODO: add default store to order from
        public Customer(string name)
        {
            this.Name = name;
            this.Id = IdSeed.ToString();
            ++IdSeed;
        }

    }
}
