using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CallTracerLibrary.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace CallTracerLibrary.DataProviders
{
    public class MongoRepository : IRepository<TraceMetadata,int>
    {
        private static MongoClient client = new MongoClient("mongodb://pendwgiiap02.pen.apac.dell.com:27017");
        private static IMongoDatabase _database = client.GetDatabase("CallTracer");
        private static IMongoCollection<TraceMetadata> collection  =_database.GetCollection<TraceMetadata>("TrackingHistoryNew");
      
        public async Task<TraceMetadata> Get(int id)
        {
            //var collection = _database.GetCollection<TraceMetadata>("TrackingHistoryNew");
            var results = collection.Find(x => x.Id == id).ToList();
            return results.FirstOrDefault();
        }
            
        public async Task<IEnumerable<TraceMetadata>> GetN(int n) //int pageSize, int pageNumber
        {

            var documents = collection.Find(Builders<TraceMetadata>.Filter.Empty).Limit(n).ToList();

            return documents;
        }

        public async Task<IEnumerable<TraceMetadata>> GetAll() 
        {

            var documents = collection.Find(Builders<TraceMetadata>.Filter.Empty).ToList();

            return documents;
        }

        public Task SaveAsync(TraceMetadata trace)
        {
           // var collection= _database.GetCollection<TraceMetadata>("TrackingHistoryNew");
            //trace.Id = (int)collection.Count(new BsonDocument(), null) + 1;
            return collection.InsertOneAsync(trace);
        }



     

    }


}


