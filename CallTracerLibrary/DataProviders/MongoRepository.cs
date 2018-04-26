using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CallTracerLibrary.Middlewares;
using CallTracerLibrary.Models;
using Microsoft.AspNetCore.Http;
using MongoDB.Bson;
using MongoDB.Driver;

namespace CallTracerLibrary.DataProviders
{
    public class MongoRepository : IRepository<TraceMetadata,AnalysisMetadata, int>
    {
        // Asia= ("mongodb://pendwgiiap02.pen.apac.dell.com:27017") US=("mongodb://AUSSWGIICACPE01.aus.amer.dell.com:27017").
        private static MongoClient _client = new MongoClient("mongodb://AUSSWGIICACPE01.aus.amer.dell.com:27017");
        private static IMongoDatabase _database = _client.GetDatabase("CallTracer");
        private static IMongoCollection<TraceMetadata> _collection = _database.GetCollection<TraceMetadata>("TrackingHistory");

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
            trace.Id = (int)_collection.Count(new BsonDocument(), null) + 1;
            return _collection.InsertOneAsync(trace);
        }

        public async Task<IEnumerable<AnalysisMetadata>> TraceAnalysis(DateTime start,DateTime stop)
        {
            List<string> uriList = new List<string>();
            List<AnalysisMetadata> analysisLlist = new List<AnalysisMetadata>();
            var query = _collection.AsQueryable<TraceMetadata>().Where(x=>x.RequestTimestamp>start && x.RequestTimestamp<stop)
                 .Select(x => x.RequestUri.ToLower())
                 .Distinct();

            foreach (var result in query)
            {
                uriList.Add(result);
            }
            uriList.Sort();
            foreach (var uri in uriList)
            {
                AnalysisMetadata metadata = new AnalysisMetadata();
                metadata.Resource = uri;

                metadata.RequestCount = _collection.AsQueryable<TraceMetadata>().Where(x => x.RequestTimestamp > start && x.RequestTimestamp < stop)
                 .Where(x => x.RequestUri.ToLower() == uri).Count();

                metadata.SuccessCount = _collection.AsQueryable<TraceMetadata>().Where(x => x.RequestTimestamp > start && x.RequestTimestamp < stop)
                 .Where(x => x.RequestUri.ToLower() == uri).Where(x=>x.Type=="2xx").Count();

                metadata.ClientFailureCount = _collection.AsQueryable<TraceMetadata>().Where(x => x.RequestTimestamp > start && x.RequestTimestamp < stop)
                .Where(x => x.RequestUri.ToLower() == uri).Where(x => x.Type == "4xx").Count();

                metadata.ServerFailureCount = _collection.AsQueryable<TraceMetadata>().Where(x => x.RequestTimestamp > start && x.RequestTimestamp < stop)
                .Where(x => x.RequestUri.ToLower() == uri).Where(x => x.Type == "5xx").Count();

                metadata.SuccessAverageResponseTime = metadata.SuccessCount > 0 ? _collection.AsQueryable<TraceMetadata>().Where(x => x.RequestTimestamp > start && x.RequestTimestamp < stop)
                   .Where(x => x.RequestUri.ToLower() == uri).Where(x => x.Type == "2xx").Average(x => x.ResponseTimeMs) : 0;

                metadata.ClientFailureAverageResponseTime = metadata.ClientFailureCount > 0 ? _collection.AsQueryable<TraceMetadata>().Where(x => x.RequestTimestamp > start && x.RequestTimestamp < stop)
                  .Where(x => x.RequestUri.ToLower() == uri).Where(x => x.Type == "4xx").Average(x => x.ResponseTimeMs) : 0;

                metadata.ServerFailureAverageResponseTime = metadata.ServerFailureCount > 0 ? _collection.AsQueryable<TraceMetadata>().Where(x => x.RequestTimestamp > start && x.RequestTimestamp < stop)
                  .Where(x => x.RequestUri.ToLower() == uri).Where(x => x.Type == "5xx").Average(x => x.ResponseTimeMs) : 0;

                analysisLlist.Add(metadata);
            }


            return analysisLlist;
        }

        

        

    }
}


