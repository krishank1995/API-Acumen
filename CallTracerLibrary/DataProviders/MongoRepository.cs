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
        // Asia= ("mongodb://pendwgiiap02.pen.apac.dell.com:27017") US=("mongodb://AUSSWGIICACPE01.aus.amer.dell.com:27017").
        private static MongoClient _client = new MongoClient("mongodb://AUSSWGIICACPE01.aus.amer.dell.com:27017"); 
        private static IMongoDatabase _database = _client.GetDatabase("CallTracer");
        private static IMongoCollection<TraceMetadata> _collection  =_database.GetCollection<TraceMetadata>("TrackingHistoryNew");
      
        public async Task<TraceMetadata> Get(int id)
        {
            var results = _collection.Find(x => x.Id == id).ToList();
            return results.FirstOrDefault();
        }
            
        public async Task<IEnumerable<TraceMetadata>> GetN(int n)
        {
            var documents = _collection.Find(Builders<TraceMetadata>.Filter.Empty).Limit(n).ToList();
            return documents;
        }

        public async Task<IEnumerable<TraceMetadata>> GetAll() 
        {
            var documents = _collection.Find(Builders<TraceMetadata>.Filter.Empty).ToList();
            return documents;
        }

        public Task SaveAsync(TraceMetadata trace)
        {
            var collection = _database.GetCollection<TraceMetadata>("TrackingHistoryNew");
            trace.Id = (int)collection.Count(new BsonDocument(), null) + 1;
            return collection.InsertOneAsync(trace);
        }
    }
}


