using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using CallTracerLibrary.Models;

namespace CallTracerLibrary.DataProviders
{
    public class MySQLSteeltoe
    {
        public static void InitializeMyContexts(IServiceProvider serviceProvider) 
        {
            if (serviceProvider == null)
            {
                throw new ArgumentNullException("serviceProvider");
            }

            using (var serviceScope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                Console.WriteLine("Ensuring database has been created...");
                var db = serviceScope.ServiceProvider.GetService<TraceMetadataContext>();
                if (!db.Database.EnsureCreated())
                {
                    Console.WriteLine("There may be another table in this database already, attempting to create with a workaround");
                    RelationalDatabaseCreator databaseCreator = (RelationalDatabaseCreator)db.Database.GetService<IDatabaseCreator>();
                    databaseCreator.CreateTables();
                }
            }

            InitializeContext(serviceProvider);
        }

        private static void InitializeContext(IServiceProvider serviceProvider)
        {
            using (var serviceScope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var db = serviceScope.ServiceProvider.GetService<TraceMetadataContext>();

                if (db.CallTraces.Any())
                {
                    return;
                }

                AddData<TraceMetadata>(db, new TraceMetadata() { Id = 1, RequestContent = "Request Content" });
                db.SaveChanges();
            }
        }

        private static void AddData<TData>(DbContext db, object item) where TData : class
        {
            db.Entry(item).State = EntityState.Added;
        }

    }
}
