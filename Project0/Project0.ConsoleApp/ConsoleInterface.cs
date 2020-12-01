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

        public void PromptUser(DbContextOptions<P0Context> dbContextOptions)
        {

            P0Repository p0Repo = new P0Repository(dbContextOptions);

            //initialize login parameters
            int customerId = 0;

            //populate initial stores and their customers from db
            List<Library.Customer> customerList = new List<Library.Customer>();
            List<Library.Store> storeList = p0Repo.GetStores(customerList);
            while (true)
            {
                //Ask if new customer
                Console.WriteLine();
                Console.WriteLine("r:\tReturning Customer");
                Console.WriteLine("n:\tNew Customer");
                var input = Console.ReadLine();

                //Returning customer
                if (input == "r")
                {
                    // compare customerID with list of all customers and login if found, else return to login
                    Console.WriteLine("Login with your Customer Id:");
                    Console.WriteLine();
                    input = Console.ReadLine();

                    //present options to returning customer
                    if (customerList.Exists(c => c.Id == Convert.ToInt32(input)))
                    {
                        Library.Customer logCustomer = customerList.Find(c => c.Id == Convert.ToInt32(input));
                        customerId = logCustomer.Id;
                        var custStores = storeList.FindAll(x => x.Customers.Exists(y => y.Id == customerId));

                        Console.WriteLine();
                        Console.WriteLine($"Welcome {logCustomer.Name}");
                        Console.WriteLine();
                        Console.WriteLine("p:\tPlace a new order at a store");
                        Console.WriteLine("s:\tSearch for an account");
                        Console.WriteLine("h:\tPrint your order history");
                        Console.WriteLine("t:\tPrint orders from a store");

                        input = Console.ReadLine();
                        if (input == "p")
                        {
                            //Create and execute a new order at a target store
                            //read target store, and selections from logged in customer, then creat this order in db
                            Library.Store storeChoice = ReadStoreChoice(storeList);
                            PrintStoreInventory(storeChoice);
                            List<Library.Product> selections = ReadSelections(storeChoice);
                            Library.Order newOrder = new Library.Order(storeChoice, logCustomer, selections);

                            //add order to db
                            p0Repo.CreateOrder(newOrder);

                            //execute order in db and update target store, add customer to store if needed
                            storeChoice = p0Repo.FillOrderDb(storeChoice, newOrder);
                            Console.WriteLine();

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
                            PrintOrderHistory(customerId, storeList);
                        }
                        else if (input == "t")
                        {
                            //print the orders this customer has made from a store
                            var storeChoice = ReadStoreChoice(storeList);
                            PrintCustOrders(customerId, storeChoice);

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
                    Console.WriteLine("Enter your full name:");
                    string customerName = Console.ReadLine();

                    p0Repo.CreateCustomer(customerName, customerList);
                    Console.WriteLine();
                    Console.WriteLine($"New account created, your Customer ID is: {Convert.ToString(customerList.Last().Id)}");
                    Console.WriteLine("Returning to Login.");
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

        //read in the selections from passed store
        private List<Library.Product> ReadSelections(Library.Store store)
        {
            List<Library.Product> prodList = new List<Library.Product>();

            Console.WriteLine();
            Console.WriteLine("Select a product by typing it's name:");
            var input = Console.ReadLine();

            while (true)
            {
                //input product found in store inventory
                if (store.Inventory.Exists(x => x.Name == input))
                {
                    Console.WriteLine();
                    Console.WriteLine("How many would you like to order?");
                    //TODO: handle invalid input
                    try
                    {
                        int quantity = Convert.ToInt32(Console.ReadLine());
                        prodList.Add(new Library.Product(input, quantity));
                    }
                    catch (System.FormatException)
                    {
                        Console.WriteLine();
                        Console.WriteLine("Please enter a number");
                        Console.WriteLine();
                    }
                }
                //user quits, return current selectiuon list
                else if (input == "q")
                {
                    break;
                }
                //unexpected input
                else
                {
                    Console.WriteLine();
                    Console.WriteLine("Product not found!");
                }
                Console.WriteLine();
                Console.WriteLine("Select another product by typing it's name, or press \"q\" to complete order.");
                input = Console.ReadLine();
            }
            return prodList;
        }

        //TODO: search for a customer at this store using customer Name
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
                Console.WriteLine();
                Console.WriteLine($"Your past orders at {store.Name}:");
                Console.WriteLine();
                //continue if no history found
                if (!store.Customers.Exists(x => x.Id == customerId))
                {
                    Console.WriteLine("No previous orders");
                    Console.WriteLine();
                    continue;
                }
                var customer = store.Customers.Find(x => x.Id == customerId);
                foreach (Library.Order o in customer.OrderHistory)
                {
                    if (store.Id == o.TargetStore.Id)
                    {
                        Console.WriteLine($"Order number: {o.OrderId}");
                        foreach (Library.Product i in o.Selections)
                        {
                            Console.WriteLine($"\tProduct: {i.Name}\t Quantity: {i.Quantity}\t DateTime: {o.Time}");
                        }
                        Console.WriteLine();
                    }
                }
            }
        }
        private void PrintCustOrders(int customerId, Library.Store store)
        {
            Console.WriteLine();
            Console.WriteLine($"Your past orders at {store.Name}:");
            Console.WriteLine();
            var customer = store.Customers.Find(x => x.Id == customerId);
            foreach (Library.Order o in customer.OrderHistory)
            {
                Console.WriteLine($"Order number: {o.OrderId}");
                foreach (Library.Product i in o.Selections)
                {
                    Console.WriteLine($"\tProduct: {i.Name}\t Quantity: {i.Quantity}\t DateTime: {o.Time}");
                }
            }
        }

        //Prints the inventory at this store
        private void PrintStoreInventory(Library.Store store)
        {
            Console.WriteLine();
            Console.WriteLine($"Items available in {store.Name}:");

            foreach (var product in store.Inventory)
            {
                Console.WriteLine($"\t{product.Name} Quantity: {product.Quantity}");
            }
        }
    }
}
