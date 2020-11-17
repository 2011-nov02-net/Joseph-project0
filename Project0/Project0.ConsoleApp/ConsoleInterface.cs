using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Project0.Data;
using System;
using System.Collections.Generic;
using System.IO;
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
            string CustomerId = "1";
            string CustomerName = "YourName";
            List<Library.Store> StoreList = p0Repo.GetStores();
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
                    //TODO: look for valid customer
                    if (input == CustomerId)
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
                            //TODO: search for a customer by name
                        }
                        else if (input == "h")
                        {
                            //TODO: Print this customer's order history
                        }
                        else if (input == "t")
                        {
                            //TODO: print the orders this customer has made from a store
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
    }
}