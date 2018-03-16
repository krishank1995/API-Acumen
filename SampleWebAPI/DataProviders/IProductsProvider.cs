using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SampleWebAPI.Models;

namespace SampleWebAPI.DataProviders
{
    public interface IProductsProvider
    {
         int AddItem(Products product);
         Products GetItemById(int id);
         IEnumerable<Products> GetAllProducts();

    }
}
