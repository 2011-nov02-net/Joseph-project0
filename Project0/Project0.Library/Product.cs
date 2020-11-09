using System;
using System.Collections.Generic;
using System.Text;

namespace Project0.Library
{
    class Product
    {
        public bool InStock { get; set; }

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
        public string Name { get;}

        //check for valid quantity when calling constructor
        public Product(string name, int initialQuantity)
        {
            this.Name = name;
            this._quantity = initialQuantity;
        }

    }
}
