using CallTracer.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CallTracerLibrary.DataProviders
{
    //public interface ITraceRepository
    //{
    //    void SaveTrace(ITraceMetadata metadata); //Save --> Can be async
    //    void RequestAllTraces(); //GetAll--> With pagination
    //    void RequestSpecificTrace(int id); //Get
    //}

    public interface IRepository<TData, TKey>
    {
        Task SaveAsync(TData value);

        // TODO: Add pagination params
        Task<IEnumerable<TData>> GetAll();

        Task<TData> Get(TKey id);
    }
}
