using System.Collections.Generic;

namespace Project0.Library
{
    public interface IStore
    {
        string Address { get; }
        List<Customer> Customers { get; set; }
        int Id { get; set; }
        List<Product> Inventory { get; set; }
        string Name { get; }

        void AddCustomer(Customer customer);
        void AddItem(string itemName, int quantity);
        List<bool> FillOrder(Order order);
        void RemoveCustomer(Customer customer);
        void RemoveItem(string itemName);
        void Restock(RestockOrder restockOrder);
    }
}