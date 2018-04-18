using CallTracerLibrary.Models;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CallTracerLibrary.DataProviders
{
    public class MySQLPCFRepository : IRepository<TraceMetadata,AnalysisMetadata, int>
    {
        private static int _counter;
        private static IServiceProvider _serviceProvider;
        private static IServiceScope _serviceScope;
        private static TraceMetadataContext _db;

       public MySQLPCFRepository(IServiceProvider serviceProvider)
        {            
            _serviceScope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope();
            _db = _serviceScope.ServiceProvider.GetService<TraceMetadataContext>();
           
            if (_db.Database.EnsureCreated() == true)
            {
                RelationalDatabaseCreator databaseCreator = (RelationalDatabaseCreator)_db.Database.GetService<IDatabaseCreator>();
            }

        }

        public static void InitializeMyContexts(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public Task<TraceMetadata> Get(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<TraceMetadata>> GetAll()
        {
            using (var context = _db)
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

        public Task<IEnumerable<TraceMetadata>> GetN(int n)
        {
            throw new NotImplementedException();
        }

        public Task SaveAsync(TraceMetadata value)
        {
           
            _db.CallTraces.Add(value);
            return  _db.SaveChangesAsync();
        }

        public Task<IEnumerable<AnalysisMetadata>> TraceAnalysis(DateTime stamp1,DateTime stamp2)
        {
            throw new NotImplementedException();
        }
    }
}
