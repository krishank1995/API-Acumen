//using Microsoft.AspNetCore.Builder;
//using Microsoft.AspNetCore.Http;
//using MongoDB.Bson;
//using MongoDB.Driver;
//using System.Linq;
//using MongoDB.Bson.IO;
//using CallTracerLibrary.DataProviders;
//using CallTracerLibrary.Models;

//namespace CallTracerLibrary.Middleware
//{
//    public static class TracingMiddlewareEndPoint
//    {
//        private static MongoClient client = new MongoClient("mongodb://pendwgiiap02.pen.apac.dell.com:27017"); //IOptions  ; Hardcoding "" --> Not allowed
//        private static IMongoDatabase database = client.GetDatabase("CallTracer");
//        private static int _defaultPageSize = 20;




//        public static IMongoCollection<BsonDocument> DbRead()
//        {
//            var collection = database.GetCollection<BsonDocument>("TrackingHistoryNew");
//            return collection;
//        }

//        public static IApplicationBuilder TraceEndPoint(this IApplicationBuilder app)
//        {
//            app.Map("/trace", OnTrace);
//            return app;
//        }

//        private static void OnTrace(IApplicationBuilder app) //is private
//        {
//            app.Run(async context =>
//            {//status?page=0&size=10-->Page numbering starts from 0

//                int pageSize, pageNumber, recordsToSkip;
//                var pageSizeStr = context.Request.Query["size"].ToString();

//                var pageNumberStr = context.Request.Query["page"].ToString();
//                int.TryParse(pageSizeStr, out pageSize);
//                int.TryParse(pageNumberStr, out pageNumber);
//                pageSize = pageSize == 0 ? pageSize = _defaultPageSize : pageSize;
//                recordsToSkip = pageSize * pageNumber;


//                var logList = DbRead();
//                var jsonWriterSettings = new JsonWriterSettings { OutputMode = JsonOutputMode.Strict };
//                var asyncDocuments = await logList.Find(Builders<BsonDocument>.Filter.Empty)
//                .ToListAsync();
//                var docs = asyncDocuments.Select(q => q.ToJson(jsonWriterSettings))
//                .Skip(recordsToSkip)
//                .Take(pageSize)
//                .ToList();
//                var json = Newtonsoft.Json.JsonConvert.SerializeObject(docs);

//                context.Response.ContentType = "Application/json";
//                await context.Response.WriteAsync(json);

//            });
//        }


//    }
//}
