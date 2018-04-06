using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CallTracerLibrary.Models;

namespace CallTracerLibrary.DataProviders
{
    public  class InMemoryRepository : IRepository<TraceMetadata, int>
    {
        private static List<TraceMetadata> _list =  new List<TraceMetadata>();

        public async Task<TraceMetadata> Get(int id)
        {
            return _list.ToList().Find(x => x.Id == id);
        }
            
        public async Task<IEnumerable<TraceMetadata>> GetAll() 
        {
            return _list;
        }

        public async Task<IEnumerable<TraceMetadata>> GetN(int n) 
        {
            return _list.Take(n);
        }

        public Task SaveAsync(TraceMetadata trace)
        {
             trace.Id = _list.Count() + 1;
            _list.Add(trace);
            return (null);  
        }
    }
}
