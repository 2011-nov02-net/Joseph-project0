using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using Xunit;
using Project0.Library;
using System.Linq;
using Project0.Data;
using Moq;

namespace XUnitTestProject0
{
    public class LibraryUnitTest
    {
        /*
        [Fact]
        public void StoreConstructorTest()
        {
            Project0.Library.Product product2 = new Product("test product2", 5);
            Product product1 = new Product("test product1", 2);
            List<Product> testInventory = new List<Product>() { product1, product2 };
            Store testStore = new Store(testInventory, "123 Fake Street", "Test Store");

            Assert.True(testStore.Inventory == testInventory);
            Assert.True(testStore.Name == "Test Store");
            Assert.True(testStore.Address == "123 Fake Street");
        }
        
        [Theory]
        [InlineData("bananas", 10)]
        [InlineData("bananananas", 0 )]//handled exception
        [InlineData("", 1000)]//unhandled exception
        public void StoreAddItemTest(string itemName, int quantity)
        {
            List<Product> testInventory = new List<Product>();
            Store testStore = new Store(testInventory, "123 Fake Street", "Test Store");

            testStore.AddItem(itemName, quantity);

            Assert.True(itemName == testStore.Inventory.First().Name);
        }

        [Fact]
        public void StoreAddCustomerTest()
        {
            Customer customer = new Customer("test Customer");
            Store testStore = new Store("123 Fake Street", "Test Store");

            testStore.AddCustomer(customer);

            Assert.True(testStore.Customers.First() == customer);
        }

        [Fact]
        public void StoreRemoveCustomerTest()
        {
            Customer customer = new Customer("test Customer");
            Store testStore = new Store("123 Fake Street", "Test Store")
            {
                Customers = {customer}
            };

            testStore.RemoveCustomer(customer);

            Assert.True(testStore.Customers.FirstOrDefault() == null);
        }

        [Fact]
        public void StoreRemoveItemTest()
        {
            string testItemName = "testItem";
            Product product = new Product(testItemName, 2);
            Store testStore = new Store("123 Fake Street", "Test Store")
            {
                Inventory = {product}
            };

            testStore.RemoveItem(testItemName);

            Assert.True(testStore.Inventory.FirstOrDefault() == null);
        }

        [Fact]
        public void StoreFillOrderTest()
        {
            Product product1 = new Product("test product1", 2);
            Customer testOrderer = new Customer("test orderer");
            List < Product >  testSelections= new List<Product>() { product1 };
            Store testStore = new Store("123 Fake Street", "Test Store")
            {
                Inventory = { new Product("test product1", 10) }
            }; 
            Order testOrder = new Order(testStore, testOrderer, testSelections);

            List<bool> ordersFilled = testStore.FillOrder(testOrder);

            Assert.True(ordersFilled.First() == true);
            Assert.True(testStore.Inventory.First().Quantity < 10);
        }

        [Fact]
        public void StoreFillOrderTest2()//will fail
        {
            Product product2 = new Product("test product2", 5);
            Customer testOrderer = new Customer("test orderer");
            List<Product> testSelections = new List<Product>() { product2 };
            Store testStore = new Store("123 Fake Street", "Test Store")
            {
                Inventory = { new Product("test product1", 10) }
            }; 
            Order testOrder = new Order(testStore, testOrderer, testSelections);

            List<bool> ordersFilled = testStore.FillOrder(testOrder);

            Assert.True(ordersFilled.First() == false);
            Assert.True(testStore.Inventory.First().Quantity == 10);
        }
        
        [Fact]
        public void StoreRestockTest()
        {
            Product product2 = new Product("test product2", 5);
            Product product1 = new Product("test product1", 2);
            Customer testOrderer = new Customer("test orderer");
            List<Product> testSelections = new List<Product>() { product1, product2 };
            Store testStore = new Store("123 Fake Street", "Test Store")
            {
                Inventory = { new Product("test product1", 10) }
            }; ;
            Order testOrder = new Order(testStore, testOrderer, testSelections);

            testStore.Restock(testOrder);

            Assert.True(testStore.Inventory.First().Quantity > 10);
        }

        [Fact]
        public void StoreRestockTest2()
        {
            Product product2 = new Product("test product2", 5);
            Product product1 = new Product("test product1", 2);
            Customer testOrderer = new Customer("test orderer");
            List<Product> testSelections = new List<Product>();
            Store testStore = new Store("123 Fake Street", "Test Store")
            {
                Inventory = { new Product("test product1", 10) }
            }; ;
            Order testOrder = new Order(testStore, testOrderer, testSelections);

            testStore.Restock(testOrder);

            Assert.True(testStore.Inventory.First().Quantity == 10);
        }
        
        [Fact]
        public void CustomerAddToOrderHistoryTest1()
        {
            Project0.Library.Product product2 = new Project0.Library.Product("test product2", 5);
            Project0.Library.Product product1 = new Project0.Library.Product("test product1", 2);
            Project0.Library.Customer testOrderer = new Project0.Library.Customer("test orderer");
            List<Project0.Library.Product> testSelections = new List<Project0.Library.Product>() { product1, product2 };
            Project0.Library.Store testStore = new Project0.Library.Store("123 Fake Street", "Test Store")
            {
                Inventory = { new Project0.Library.Product("test product1", 10) }
            }; ;
            Project0.Library.Order testOrder = new Project0.Library.Order(testStore, testOrderer, testSelections);

            testOrderer.AddToOrderHistory(testOrder);

            Assert.True(testOrderer.OrderHistory.First() == testOrder);
        }
        */
        [Fact]
        public void GetDbStoreCustomerTest()
        {
            //arrange
            var mockStore = new Mock<Project0.Library.Store>();
            //mockStore.Setup(x => x.Id = 3);
            
            var mockRepository = new Mock<IP0Repository>();
            mockRepository.Setup(x =>
                x.GetStoreCustomer(It.IsAny<string>(), It.IsAny<Project0.Library.Store>()))
                .Returns(3);    //CustomerId == 3
            
            //act
            int CustomerId = mockRepository.Object.GetStoreCustomer("testName", mockStore.Object);

            //assert
            Assert.Equal(3,CustomerId);
        }

        [Fact]
        public void CreateDbCustomerTest()
        {
            //arrange
            var mockCustomers = new Mock<List<Customer>>();

            var mockRepository = new Mock<IP0Repository>();
            mockRepository.Setup(x =>
                x.CreateCustomer(It.IsAny<string>(), It.IsAny<List<Customer>>()))
                    .Verifiable();

            //act
            mockRepository.Object.CreateCustomer("testName", mockCustomers.Object);

            //assert
            Assert.IsAssignableFrom<Customer>(mockCustomers.Object.First());
            mockRepository.Verify(x => x.CreateCustomer(It.IsAny<string>(), It.IsAny<List<Customer>>()), Times.Once);
        }
    }
}
