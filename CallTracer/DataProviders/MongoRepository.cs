using CallTracer.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CallTracer.DataProviders
{
    public class MongoRepository:ITraceRepository
    {
        private static MongoClient client = new MongoClient("mongodb://pendwgiiap02.pen.apac.dell.com:27017");
        private static IMongoDatabase _database = client.GetDatabase("CallTracer");

        public void RequestAllTraces()
        {
            throw new NotImplementedException();
        }

        public void RequestSpecificTrace(int id)
        {
            throw new NotImplementedException();
        }

        public void SaveTrace(ITraceMetadata trace)
        {
            var collection = _database.GetCollection<TraceMetadata>("TrackingHistory");
            trace.Id = (int)collection.Count(new BsonDocument(), null) + 1;
            collection.InsertOneAsync(trace);
           // return (product.Id);
        }

       
    }
}
