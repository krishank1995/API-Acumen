using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CallTracerLibrary.Models;

namespace CallTracerLibrary.DataProviders
{
   public  class InMemoryRepository : IRepository<TraceMetadata, int>
    {
        private static List<TraceMetadata> _list =  new List<TraceMetadata>();
                                                                        //private TraceMetadata _trace = new TraceMetadata();



        public async Task<TraceMetadata> Get(int id)
        {
            return _list.ToList().Find(x => x.Id == id);
        }

       

        public async Task<IEnumerable<TraceMetadata>> GetAll() // ?? Should method be made asynchronous
        {
            return _list;
        }

        public async Task<IEnumerable<TraceMetadata>> GetN(int n) // ?? Should method be made asynchronous
        {
            return _list.Take(n);
        }

        public Task SaveAsync(TraceMetadata trace)
        {
            //trace.Id = _list.Count() + 1;
            _list.Add(trace);
            return (null);  
        }

      
    }
}
