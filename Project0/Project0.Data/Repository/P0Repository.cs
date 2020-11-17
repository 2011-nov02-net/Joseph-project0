using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Project0.Data
{
    public class P0Repository 
    {
        //private readonly P0Context _dbContext;
        private readonly DbContextOptions<P0Context> _dbContextOptions;

        public P0Repository(DbContextOptions<P0Context> contextOptions)
        {
            _dbContextOptions = contextOptions;
        }

        /*
        public void AddOrder(Library.Order appOrder)
        {
            using var context = new P0Context(_dbContextOptions);
            //map library order to db
            var dbOrder = new Order()
            {
                Store = _dbContext.Stores.First(s => s.Id == appOrder.TargetStore.Id),
                Customer = _dbContext.StoreCustomers.First(c => c.Id == appOrder.Orderer.Id),
                Time = appOrder.Time
            };

            //map all items in the order to db
            foreach(Library.Product selection in appOrder.Selections)
            {
                var dbItem = new Item()
                {
                    Product = _dbContext.Products.First(p => p.Id == selection.Id),
                    Quantity = selection.Quantity,
                    Store = _dbContext.Stores.First(s => s.Id == appOrder.TargetStore.Id),
                    Order = dbOrder
                };
                _dbContext.Add(dbItem);
            }
            _dbContext.Add(dbOrder);
            _dbContext.SaveChanges();
        }
        //retrieve product from db
        public Library.Product GetProduct(string productName)
        {
            using var context = new P0Context(_dbContextOptions);
            var dbProduct = context.Products.First(p => p.Name == productName);

            var appProduct = new Library.Product()
                { Id = dbProduct.Id, Name = productName };
            _dbContext.SaveChanges();
            return appProduct;
        }
        
        //creates a store's inventory from the db
        public void PopulateInventory(Library.Store store)
        {
            using var context = new P0Context(_dbContextOptions);
            var dbItems = context.Items
                .Where(x => x.StoreId == store.Id)
                .Include(x => x.Product)
                .ToList();
            foreach (var item in dbItems)
            {
                store.AddItem(item.Product.Name, (int)item.Quantity);
            };
        }
        */

        public bool CustomerExists(string customerName, Library.Store store)
        {
            // if customer has ordered from store return true
            using var context = new P0Context(_dbContextOptions);
            bool exists = false;

            var dbOrders= context.Orders
               .Include(o => o.Customer)
               .ToList();
            foreach (var order in dbOrders)
            {
                if (order.StoreId == store.Id && order.Customer.Name == customerName)
                    exists = true;
            }

            return exists;
        }
        public int GetStoreCustomer(string customerName, Library.Store store)
        {
            //if customer has ordered from store return appCustomerid
            //return of 0 indicates customer wasnt found
            using var context = new P0Context(_dbContextOptions);
            int customerId = 0;

            var dbOrders = context.Orders
               .Include(o => o.Customer)
               .ToList();
            foreach (var order in dbOrders)
            {
                if (order.StoreId == store.Id && order.Customer.Name == customerName)
                    customerId = (int)order.CustomerId;
            }
            return customerId;
        }

        //return a list of appstores from the db
        //should be called first to populate stores
        public List<Library.Store> GetStores()
        {
            using var context = new P0Context(_dbContextOptions);
            List<Library.Store> storeList = new List<Library.Store>();

            var dbStores = context.Stores
                .Include(s=> s.Items)
                    .ThenInclude(i=>i.Product)
                .Include(s=> s.Orders)
                    .ThenInclude(o => o.Customer)
                .ToList();

            //list of all customers
            List<Library.Customer> allCustomers = new List<Library.Customer>();
            //creates an appstore for each store in the db
            foreach (var store in dbStores)
            {
                Library.Store appStore = new Library.Store(store.Location, store.Name);
                foreach(var item in store.Items)
                {
                    appStore.AddItem(item.Product.Name, (int)item.Quantity);
                }
                foreach (var order in store.Orders)
                {
                    //check if customer is already created
                    bool created = false;
                    foreach (var customer in allCustomers)
                    {
                        if (customer.Name == order.Customer.Name)
                        {
                            created = true;
                            customer.AddToOrderHistory(GetOrder(order.Id, appStore, customer));
                        }
                    }
                    //if not, create new customer
                    if (!created)
                    {
                        Library.Customer newCustomer = new Library.Customer(order.Customer.Name);
                        appStore.AddCustomer(newCustomer);
                        allCustomers.Add(newCustomer);
                        newCustomer.AddToOrderHistory(GetOrder(order.Id, appStore, newCustomer));
                    }
                }
                storeList.Add(appStore);
            }
            //could return allCustomers
            return storeList;
        }

        public List<int> GetOrdersFromStore(Library.Store store, Library.Customer customer)
        {
            //TODO: Output a list of Order Ids from customer to this store
            using var context = new P0Context(_dbContextOptions);
            List<int> OrdIds = new List<int>();


            return OrdIds;
        }
        public List<int> GetCustomerOrders(Library.Customer customer)
        {
            //TODO: Output a list of Order Ids from customer to all stores
            using var context = new P0Context(_dbContextOptions);
            List<int> OrdIds = new List<int>();


            return OrdIds;
        }

        public void CreateCustomer(Library.Customer customer)
        {
            //TODO: Enter a new customer into db
            using var context = new P0Context(_dbContextOptions);

            context.SaveChanges();
        }

        public void CreateOrder(Library.Order order)
        {
            //TODO: Enter a new order into db
            using var context = new P0Context(_dbContextOptions);
 
            context.SaveChanges();
        }

        //returns an appOrder from the db
        public Library.Order GetOrder(int orderId,Library.Store appStore,Library.Customer appCustomer)
        {
            using var context = new P0Context(_dbContextOptions);

            //create a product list for the order
            List<Library.Product> selections = new List<Library.Product>();
            var dbItems = context.Items
                .Include(i => i.Product)
                .Where(i => i.OrderId == orderId);
            foreach (var item in dbItems)
            {
                foreach (var product in appStore.Inventory) {
                    if (item.Product.Name != product.Name)
                    {
                        selections.Add(new Library.Product(item.Product.Name, (int)item.Quantity));
                    }
                }
            }
                
            //create the order
            var dbOrder = context.Orders
                .First(x => x.Id == orderId);

            Library.Order newOrder = new Library.Order()
            {
                TargetStore = appStore,
                Orderer = appCustomer,
                Time = dbOrder.Time,
                Selections = selections
            };
            return newOrder;
        }

        public void PrintOrderHistory()
        {
            using var context = new P0Context(_dbContextOptions);

            IQueryable<Data.StoreCustomer> customers = context.StoreCustomers
                .Include(c => c.Orders)
                    .ThenInclude(o => o.Items)
                .OrderBy(c => c.Id)
                .Take(50);
            foreach (var customer in customers)
            {
                Console.WriteLine($" CustomerName: {customer.Name}");
                foreach (var order in customer.Orders)
                {
                    Console.WriteLine($" Order: {order.Id}");
                }
            }
        }
        

    }
}
