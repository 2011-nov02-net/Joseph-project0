using System;
using System.Collections.Generic;
using System.Text;

namespace Project0.Library
{
    /// <summary>
    ///  Provides data fields representing a particular product
    /// </summary>
    public class Product
    {
        public bool InStock { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }

        public int Id;
        private static int _Id = 1;

        private int _quantity;
        public int Quantity
        {
            get { return _quantity; }
            set
            {
                if(value <= 0)
                {
                    InStock = false;
                    _quantity = 0;
                }
                else
                {
                    _quantity = value;
                }
            }
        }

        //TODO:check for valid quantity when calling constructor
        public Product(string name, int initialQuantity)
        {
            this.Name = name;
            this._quantity = initialQuantity;
            this.Id = _Id;
            ++_Id;
        }
        public Product()
        {
        }

    }
}
