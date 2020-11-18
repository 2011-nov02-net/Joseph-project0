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
        public List<Library.Store> GetStores(List<Library.Customer> allCustomers)
        {
            using var context = new P0Context(_dbContextOptions);
            List<Library.Store> storeList = new List<Library.Store>();

            var dbStores = context.Stores
                .Include(s=> s.Items)
                    .ThenInclude(i=>i.Product)
                .Include(s=> s.Orders)
                    .ThenInclude(o => o.Customer)
                .ToList();

            //creates an appstore for each store in the db
            foreach (var dbStore in dbStores)
            {
                Library.Store appStore = new Library.Store(dbStore.Location, dbStore.Name);
                foreach(var item in dbStore.Items)
                {
                    appStore.AddItem(item.Product.Name, (int)item.Quantity);
                }
                foreach (var order in dbStore.Orders)
                {
                    //check if customer is already created
                    bool created = false;
                    foreach (var customer in allCustomers)
                    {
                        if (customer.Name == order.Customer.Name)
                        {
                            created = true;
                            appStore.AddCustomer(customer);
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

        /// <summary>
        /// Enter a new customer into db, and app
        /// </summary>
        /// <param name="customerName"></param>
        public void CreateCustomer(string customerName, List<Library.Customer> customers)
        {
            //add to app
            Library.Customer newCustomer = new Library.Customer(customerName);
            customers.Add(newCustomer);

            //add to db
            using var context = new P0Context(_dbContextOptions);

            newCustomer.Id = context.StoreCustomers.OrderBy(x => x.Id).Last().Id + 1;
            var dbCustomer = new StoreCustomer(){Name = customerName};
            context.Add(dbCustomer);
            context.SaveChanges();
        }

        /// <summary>
        /// Enter a new order into db
        /// </summary>
        /// <param name="appOrder"></param>
        public void CreateOrder(Library.Order appOrder)
        {
            using var context = new P0Context(_dbContextOptions);
            //map library order to db
            var dbOrder = new Order()
            {
                Store = context.Stores.First(s => s.Id == appOrder.TargetStore.Id),
                Customer = context.StoreCustomers.First(c => c.Id == appOrder.Orderer.Id),
                Time = appOrder.Time
            };

            //map all items in the order to db
            foreach (Library.Product selection in appOrder.Selections)
            {
                //create a new item, Store = null unless item is part of an inventory
                var dbItem = new Item()
                {
                    Product = context.Products.First(p => p.Name == selection.Name),
                    Quantity = selection.Quantity,
                    Store = null,
                    //Store = context.Stores.First(s => s.Id == appOrder.TargetStore.Id),
                    Order = dbOrder
                };
                context.Add(dbItem);
            }
            context.Add(dbOrder);
            context.SaveChanges();
        }
        /// <summary>
        /// //execute order in db and update passed Store
        /// </summary>
        /// <param name="storeChoice"></param>
        /// <param name="appOrder"></param>
        /// <returns></returns>
        public Library.Store FillOrderDb(Library.Store storeChoice, Library.Order appOrder)
        {
            using var context = new P0Context(_dbContextOptions);
            List<bool> successList = storeChoice.FillOrder(appOrder);
            int successListIndex = 0;

            //go ahead and grab everything
            var dbStore = context.Stores
                .Include(s => s.Items)
                    .ThenInclude(i => i.Product)
                .Include(s => s.Orders)
                    .ThenInclude(o => o.Customer)
                .First(s => s.Id == storeChoice.Id);

            //get store inventory
            var dbItems = context.Items.Where(s => s.StoreId == storeChoice.Id);

            //find all items in inventory matching product.Name appOrderId then decrement quantity and update appStore
            foreach (var product in appOrder.Selections)
            {
                //find first item in dbItems that has the same name, and is an inventory
                var dbInvItem = dbItems.FirstOrDefault(i => i.Product.Name == product.Name);

                //update the db when fillOrder is successful
                if (successList.ElementAt(successListIndex)) // == appOrder.OrderId &&
                {
                    dbInvItem.Quantity -= product.Quantity;
                    context.Update(dbInvItem);
                    //storeChoice.Inventory.First(x=>x.Name == product.Name).Quantity = (int)dbInvItem.Quantity;
                }
                successListIndex++;
            }

            context.SaveChanges();
            return storeChoice;
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
                if(selections.Count == 0)
                    selections.Add(new Library.Product(item.Product.Name, (int)item.Quantity));

                for(int i = 0; i < selections.Count; ++i)
                {
                    if (item.Product.Name != selections.ElementAt(i).Name)
                        selections.Add(new Library.Product(item.Product.Name, (int)item.Quantity));
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
    }
}
