using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using SampleWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SampleWebAPI.DataProviders
{
    public class ProductsProviderMongo:IProductsProvider
    {
        private static MongoClient client = new MongoClient("mongodb://pendwgiiap02.pen.apac.dell.com:27017");
        private static IMongoDatabase _database = client.GetDatabase("CallTracer");
        private static IMongoCollection<Products> collection = _database.GetCollection<Products>("Products");

        public int AddItem(Products product)
        {
     
            product.Id = (int)collection.Count(new BsonDocument(), null) + 1;
            collection.InsertOneAsync(product);
            return (product.Id);
        }

       public Products GetItemById(int id)
        {

            var results = collection.Find(x => x.Id == id).ToList();
            return results.FirstOrDefault();
        }   

        public IEnumerable<Products> GetAllProducts()
        {

            var documents = collection.Find(Builders<Products>.Filter.Empty).ToList();
            return documents;

        }

        public void Delete (int id)
        {
            collection.DeleteOne(x => x.Id == id);
        }

        public void Update(int id,Products product)
        {
            product.Id = id;
            var replaceOneResult =collection.ReplaceOne(x => x.Id == product.Id, product);
        }


    }
}
    