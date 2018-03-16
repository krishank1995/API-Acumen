using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SampleWebAPI.Models
{
    public class Products
    {
        public Products() { }
        public Products(string name, int price, bool activeStatus)
        {
            Name = name;
            Price = price;
            ActiveStatus = activeStatus;

        }

        // Attributes
        public string Name { get; set; }
        public int Id { get; set; }
        public int Price { get; set; }
        public bool ActiveStatus { get; set; }
      //  public IEnumerable<string> Address { get; set; }
    }
}
