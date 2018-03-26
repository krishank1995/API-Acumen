using CallTracerLibrary.Models;//Del this
using MongoDB.Driver; //Del this
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CallTracerLibrary.DataProviders
{
    public interface IRepository<TData, TKey>
    {
        Task SaveAsync(TData value);

       
        Task<IEnumerable<TData>> GetN(int n); //int pageSize,int pageNumber

        Task<IEnumerable<TData>> GetAll();

        Task<TData> Get(TKey id);


        
    }
}
