using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Project0.Data;
using Project0.Library;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;


namespace Project0.ConsoleApp
{
    class Program
    {
        static DbContextOptions<P0Context> s_dbContextOptions;
        /// <summary>
        /// Entry point for the application, contains control for how the
        /// user ineracts with the application, and most IO behaviour
        /// </summary>

        static void Main(string[] args)
        {
            
            using var logStream = new StreamWriter("ef-logs.txt");

            var optionsBuilder = new DbContextOptionsBuilder<P0Context>();
            optionsBuilder.UseSqlServer(GetConnectionString());
            optionsBuilder.LogTo(logStream.WriteLine, LogLevel.Information);
            s_dbContextOptions = optionsBuilder.Options;

            ConsoleInterface Ci = new ConsoleInterface();
            Ci.PromptUser(s_dbContextOptions);

            //PrintOrderHistory();

            /*//old stuff
            List<Library.Product> prodList = new List<Library.Product>();

            Library.Store aStore = new Library.Store(prodList, "123 Fake St.", "77840");

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
            List<Library.Product> stockList = new List<Library.Product>
            {
                //     new Product()
            };

            Library.Order stockOrder = new Library.Order(stockList);
            // aStore.Restock();
            */
        }
        static void PrintOrderHistory()
        {
            using var context = new P0Context(s_dbContextOptions);

            IQueryable<Data.StoreCustomer> customers = context.StoreCustomers
                .Include(c => c.Orders)
                    .ThenInclude(o => o.Items)
                .OrderBy(c => c.Id)
                .Take(50);
            foreach(var customer in customers)
            {
                Console.WriteLine($" CustomerName: {customer.Name}");
                foreach (var order in customer.Orders)
                {
                    Console.WriteLine($" Order: {order.Id}");
                }
            }
        }

        static string GetConnectionString()
        {
            string path = "../../../../../../p0-connection-string.json";
            string json;
            try
            {
                json = File.ReadAllText(path);
            }
            catch (IOException)
            {
                Console.WriteLine($"required file {path} not found. should just be the connection string in quotes.");
                throw;
            }
            string connectionString = JsonSerializer.Deserialize<string>(json);
            return connectionString;    
        }

    }
}
