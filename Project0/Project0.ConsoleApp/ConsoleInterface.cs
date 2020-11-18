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
                Console.WriteLine();
                Console.WriteLine("r:\tReturning Customer");
                Console.WriteLine("n:\tNew Customer");
                var input = Console.ReadLine();

                //Ask if new customer
                if (input == "r")
                {
                    // compare customerID with list of all customers and login if found, else return to login
                    Console.WriteLine("Login with your Customer Id:");
                    Console.WriteLine();
                    input = Console.ReadLine();

                    //present options to returning customer
                    //if (input == Convert.ToString(customerId))
                    if (customerList.Exists(c => c.Id == Convert.ToInt32(input)))
                    {
                        Library.Customer logCustomer = customerList.Find(c => c.Id == Convert.ToInt32(input));
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
                            //PrintOrderHistory(logCustomer.Id, storeList);
                            Library.Store storeChoice = ReadStoreChoice(storeList);
                            PrintStoreInventory(storeChoice);
                            List<Library.Product> selections = ReadSelections(storeChoice);
                            Library.Order newOrder = new Library.Order(storeChoice, logCustomer, selections);

                            //add order to db
                            p0Repo.CreateOrder(newOrder);

                            //execute order in db and update target store, add customer to store if needed
                            storeChoice = p0Repo.FillOrderDb(storeChoice,newOrder);
                            //PrintOrderHistory(logCustomer.Id, storeList); 
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
                            PrintOrderHistory(customerId, custStores);
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
                if(store.Inventory.Exists(x => x.Name == input))
                {
                    Console.WriteLine();
                    Console.WriteLine("How many would you like to order?");
                    //TODO: handle invalid input
                    int quantity = Convert.ToInt32(Console.ReadLine());
                    prodList.Add(new Library.Product(input, quantity));
                }
                //user quits, return current selectiuon list
                else if(input == "q")
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
                    continue;
                }
                var customer = store.Customers.Find(x => x.Id == customerId);
                foreach(Library.Order o in customer.OrderHistory)
                {
                    if (store.Id == o.TargetStore.Id)
                    {
                        Console.WriteLine($"Order number: {o.OrderId}");
                        foreach (Library.Product i in o.Selections)
                        {
                            Console.WriteLine($"\tProduct: {i.Name}\t Quantity: {i.Quantity}\t DateTime: {o.Time}");
                        }
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

            foreach(var product in store.Inventory)
            {
                Console.WriteLine($"\t{product.Name} Quantity: {product.Quantity}");
            }
        }
    }
}

