using Project0.Library;
using System.Collections.Generic;

namespace Project0.Data
{
    public interface IP0Repository
    {
        void CreateCustomer(string customerName, List<Customer> customers);
        void CreateOrder(Library.Order appOrder);
        bool CustomerExists(string customerName, Library.Store store);
        Library.Store FillOrderDb(Library.Store storeChoice, Library.Order appOrder);
        Library.Order GetOrder(int orderId, Library.Store appStore, Customer appCustomer);
        int GetStoreCustomer(string customerName, Library.Store store);
        List<Library.Store> GetStores(List<Customer> allCustomers);
    }
}