using CallTracerLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CallTracerLibrary.DataProviders
{
    public class MySQLRepository:IRepository<TraceMetadata,int>

    {
        public async Task<TraceMetadata> Get(int id)
        {
            using (var context = new TraceMetadataContext())
            {
                List<TraceMetadata> list = new List<TraceMetadata>();
                var result = context.CallTraces;

                foreach (var trace in result)
                {
                    if(trace.Id==id)
                    {
                        list.Add(trace);
                    }
                }

                return list.FirstOrDefault();
            }
        }

        public async Task<IEnumerable<TraceMetadata>> GetAll()
        {
            using (var context = new TraceMetadataContext())
            {
                List<TraceMetadata> list = new List<TraceMetadata>();
                var result = context.CallTraces;

                foreach (var trace in result)
                {
                    list.Add(trace);
                }

                return list;
            }

        }

        public  Task<IEnumerable<TraceMetadata>> GetN(int n)
        {
            throw new NotImplementedException();
        }

        public Task SaveAsync(TraceMetadata value)
        {
            using (var context = new TraceMetadataContext())
            {
                context.Database.EnsureCreated();
                context.CallTraces.Add(value);
                return context.SaveChangesAsync();
            }
        }
    }
}




