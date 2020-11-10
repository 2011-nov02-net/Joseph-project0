using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using Xunit;
using Project0.Library;

namespace XUnitTestProject0
{
    public class UnitTest1
    {
        
        public void StoreAddItemTest()
        {
            [Fact]
            [InlineData("bananas", 10)]
            [InlineData("bananananas", 0 )]
            [InlineData("", 1000)]
            public void StoreAddItemTest(string itemName, int quantity)
            {
                ICollection<Product> testInventory;

                Store testStore = new Store(testInventory, "123 Fake Street", "77840");
                testStore.AddItem(itemName, quantity);
            }
        }
    }
}
