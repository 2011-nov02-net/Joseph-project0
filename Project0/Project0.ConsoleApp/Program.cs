using Project0.Library;
using System;
using System.Collections.Generic;

namespace Project0.ConsoleApp
{
    class Program
    {
        /// <summary>
        /// Entry point for the application, contains control for how the
        /// user ineracts with the application, and most IO behaviour
        /// </summary>
        static void Main(string[] args)
        {
            List<Product> prodList = new List<Product>();

            Store aStore = new Store(prodList, "123 Fake St.", "77840");

            //add some customers
            Customer Ruth = new Customer("Ruth Ginsberg");
            Customer Clarence = new Customer("Clarence Thomas");
            Customer Sam = new Customer("Samuel Alito");
            Customer John = new Customer("John Roberts");
            aStore.AddCustomer(Ruth);
            aStore.AddCustomer(Clarence);
            aStore.AddCustomer(Sam);
            aStore.AddCustomer(John);

            //add some inventory items
            List<Product> stockList = new List<Product>
            {
                new Product()
            };

            Order stockOrder = new Order(stockList);
            aStore.Restock();

        }
    }
}
