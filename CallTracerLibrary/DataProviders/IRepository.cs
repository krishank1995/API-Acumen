using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CallTracerLibrary.DataProviders
{
    public interface IRepository<TData,TAnalysis, TKey>
    {
        Task SaveAsync(TData value);
        Task<IEnumerable<TData>> GetN(int n);
        Task<IEnumerable<TData>> GetAll();
        Task<TData> Get(TKey id);
        Task<IEnumerable<TAnalysis>> TraceAnalysis(DateTime stamp1, DateTime stamp2);
    }
}
