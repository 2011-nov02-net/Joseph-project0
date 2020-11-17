using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Project0.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;

namespace Project0.ConsoleApp
{
    class ConsoleInterface
    {
        /*
        public ConsoleInterface()
        {

            using var logStream = new StreamWriter("ef-logs.txt");

            var optionsBuilder = new DbContextOptionsBuilder<P0Context>();
            optionsBuilder.UseSqlServer(GetConnectionString());
            optionsBuilder.LogTo(logStream.WriteLine, LogLevel.Information);
            var _dbContextOptions = optionsBuilder.Options;
            P0Repository p0Repo = new P0Repository(optionsBuilder.Options);


            var optionsBuilder = new DbContextOptionsBuilder<P0Context>();
            static DbContextOptions<P0Context> s_dbContextOptions;
            s_dbContextOptions = optionsBuilder.Options;

        }
        */

        public void PromptUser(DbContextOptions<P0Context> dbContextOptions)
        {
            

            P0Repository p0Repo = new P0Repository(dbContextOptions);
            //customerId = 1 is valid for testing
            int customerId = 2;
            string CustomerName = "YourName";
            List<Library.Store> storeList = p0Repo.GetStores();
            while (true)
            {
                Console.WriteLine();
                Console.WriteLine("r:\tReturning Customer");
                Console.WriteLine("n:\tNew Customer");
                var input = Console.ReadLine();
                //present customer options
                if (input == "r")
                {
                    Console.WriteLine("Login with your Customer Id:");
                    Console.WriteLine();
                    input = Console.ReadLine();
                    //TODO: look for valid customer login
                    var custStores = storeList.FindAll(x => x.Customers.Exists(y => y.Id == customerId));
                    if (input == Convert.ToString(customerId))
                    {
                        Console.WriteLine();
                        Console.WriteLine($"Welcome {CustomerName}");
                        Console.WriteLine();
                        Console.WriteLine("p:\tPlace a new order");
                        Console.WriteLine("s:\tSearch for an account");
                        Console.WriteLine("h:\tPrint your order history");
                        Console.WriteLine("t:\tPrint orders from a store");

                        input = Console.ReadLine();
                        if (input == "p")
                        {
                            // Create and execute a new order
                            Library.Order order = new Library.Order();
                            List<string> itemName = new List<string>();
                            List<int> itemQuantity = new List<int>();
                            while (input != "q")
                            {

                                //get product name and quantity from input
                                Console.WriteLine();
                                Console.WriteLine("Enter a product:");
                                string productName = Console.ReadLine();
                                Library.Product product = null;
                                try
                                {
                                    //product = p0Repo.GetProduct(productName);
                                }
                                catch (ArgumentException)
                                {
                                    Console.WriteLine($"Couldn't find {productName} in inventory.");
                                }
                                Console.WriteLine("Enter a quantity:");
                                string tryQuantity = Console.ReadLine();
                                int quantity = 0; //definitely not ok 
                                try
                                {
                                    quantity = Int32.Parse(tryQuantity);
                                    Console.WriteLine(quantity);
                                }
                                catch (FormatException)
                                {
                                    Console.WriteLine($"not a valid quantity: '{quantity}'");
                                }
                                //add inputs to lists
                                itemName.Add(productName);
                                itemQuantity.Add(quantity);
                                Console.WriteLine("Press \"q\" if order is complete");
                                Console.WriteLine("Otherwise press enter to continue ordering");
                                input = Console.ReadLine();
                            }
                            for (int i = 0; i < itemName.Count; ++i)
                            {

                            }

                        }
                        else if (input == "s")
                        {
                            // search for a customer by name
                            var storeChoice = ReadStoreChoice(storeList);
                            string customerName = ReadCustomer();
                            if (storeChoice.Customers.Exists(x => x.Name == customerName))
                            {
                                Console.WriteLine();
                                Console.WriteLine($"{customerName} shops at {storeChoice.Name}");
                            }
                            else
                            {
                                Console.WriteLine();
                                Console.WriteLine($"{customerName} does not shop at {storeChoice.Name}");
                            }

                        }
                        else if (input == "h")
                        {
                            //Print this customer's order history
                            PrintOrderHistory(customerId, custStores);
                        }
                        else if (input == "t")
                        {
                            //TODO: print the orders this customer has made from a store
                           // var custStores = storeList.FindAll(x => x.Customers.Exists(y => y.Id == customerId));

                        }
                        else
                        {
                            Console.WriteLine();
                            Console.WriteLine();
                        }
                    }
                    else
                    {
                        Console.WriteLine();
                        Console.WriteLine("Invalid CustomerId, Returning to Login");
                        Console.WriteLine();
                    }
                }
                //create a new customer
                else if (input == "n")
                {
                    //TODO: create a new customer
                    Console.WriteLine();
                    Console.WriteLine("New account created, returning to Login:");
                    Console.WriteLine();
                }
            }
        }
        //prompt the user to pick a store
        private Library.Store ReadStoreChoice(List<Library.Store> stores)
        {
            Library.Store storeChoice;
            while (true)
            {
                Console.WriteLine();
                Console.WriteLine("Select a Store by typing it's name:");
                foreach (var store in stores)
                {
                    Console.WriteLine($"\t{store.Name}");
                }
                var input = Console.ReadLine();

                if (stores.Exists(x => x.Name == input))
                {
                    storeChoice = stores.Find(x => x.Name == input);
                    break;
                }
            }
            return storeChoice;
        }

        private string ReadCustomer()
        {
            Console.WriteLine();
            Console.WriteLine("Search for a customer at this store by typing a full name:");
            var custName = Console.ReadLine();
            return custName;
        }
        private void PrintOrderHistory(int customerId, List<Library.Store> custStores)
        {
            Console.WriteLine();
            foreach (Library.Store store in custStores)
            {
                Console.WriteLine($"Your past orders at {store.Name}:");
                Console.WriteLine();
                var customer = store.Customers.Find(x => x.Id == customerId);
                foreach(Library.Order o in customer.OrderHistory)
                {   //WIP
                    foreach(Library.Product i in o.Selections)
                    {
                        Console.WriteLine($"\t Product: {i.Name}\t\tQuantity: {i.Quantity}\t DateTime: {o.Time}");
                    }
                }
            }
        }
    }
}

