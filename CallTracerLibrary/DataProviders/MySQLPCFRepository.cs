using CallTracerLibrary.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CallTracerLibrary.DataProviders
{
    public class MySQLPCFRepository : IRepository<TraceMetadata, int>
    {
        private static int _counter;
        private static IServiceProvider _serviceProvider;
        private static IServiceScope _serviceScope;
        private static TraceMetadataContext _db;

       public MySQLPCFRepository(IServiceProvider serviceProvider)
        {
            
            _serviceScope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope();
            _db = _serviceScope.ServiceProvider.GetService<TraceMetadataContext>();

            
            if (_db.Database.EnsureCreated()==false)
            {
                RelationalDatabaseCreator databaseCreator = (RelationalDatabaseCreator)_db.Database.GetService<IDatabaseCreator>();
                databaseCreator.CreateTables();
            }
            //InitializeContext(serviceProvider);
        }


        public static void InitializeMyContexts(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        //private static void AddData<TData>(DbContext db, object item) where TData : class
        //{
        //    db.Entry(item).State = EntityState.Added;
        //}


        public Task<TraceMetadata> Get(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<TraceMetadata>> GetAll()
        {
            using (var context = _db)
            {
                List<TraceMetadata> list = new List<TraceMetadata>();
                var result = context.CallTrace;

                foreach (var trace in result)
                {
                    list.Add(trace);
                }

                return list;
            }
        }

        public Task<IEnumerable<TraceMetadata>> GetN(int n)
        {
            throw new NotImplementedException();
        }

        public Task SaveAsync(TraceMetadata value)
        {
            _counter++;
            value.Id = _counter;
            _db.CallTrace.Add(value);
            // _db.Entry(value).State = EntityState.Added;
             return  _db.SaveChangesAsync();
        }
    }
}
