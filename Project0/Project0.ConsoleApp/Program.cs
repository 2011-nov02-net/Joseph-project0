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
