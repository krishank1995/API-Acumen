//using System;
//using System.Collections.Generic;
//using System.Data.Entity;
//using System.Text;
//using System.Threading.Tasks;
//using CallTracerLibrary.Models;
//using MySql.Data;
//using MySql.Data.MySqlClient;

using CallTracerLibrary.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using MySql.Data.EntityFrameworkCore.Extensions;

namespace CallTracerLibrary.DataProviders
{
    public class MySQLRepository:DbContext,IRepository<TraceMetadata,int>

    {
        //public MySQLRepository(DbContextOptions options) : base(options)
        //{

        //}
        
        public DbSet<TraceMetadata> CallTrace { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySQL("server=localhost;database=ApiCallTraces;user=root;password=password;SslMode=none;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);



            modelBuilder.Entity<TraceMetadata>(entity =>
            {
                entity.HasKey(e => e.Id);
                //entity.Property(e => e.Name).IsRequired();

            });
        }



        public async Task<TraceMetadata> Get(int id)
        {
           // using (var context = new TraceMetadataContext())
            {
                List<TraceMetadata> list = new List<TraceMetadata>();
                var result = CallTrace;

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
           // using (var context = new TraceMetadataContext())
            {
                List<TraceMetadata> list = new List<TraceMetadata>();
                var result = CallTrace;

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
           // using (var context = new TraceMetadataContext())
            {
                // Creates the database if not exists
                Database.EnsureCreated();
   
                CallTrace.Add(value);

                // Saves changes
                return SaveChangesAsync();
            }
        }

    }




}


//Create Operation

//private static string _connectionString;
//private static MySqlConnection _connection;
//private static MySqlDataReader _reader;
//private static MySqlCommand _command;

//public MySQLRepository()
//{
//    string _connectionString = "datasource=localhost;port=3306;username=root;password=password;";
//    _connection = new MySqlConnection(_connectionString);

//}

//List<Users> user = new List<Users>();

//public async Task SaveAsync()
//{

//var query = "INSERT INTO TEMPDB.TEMP VALUES (4,'Arjun')";
//_command = new MySqlCommand(query, _connection);
//_connection.Open();
//await _command.ExecuteReaderAsync();
//await _connection.CloseAsync();


//}

//public static void Method()
//{
//    using (var context = new UsersContext())
//    {
//        // Creates the database if not exists
//        context.Database.EnsureCreated();

//        // Adds a publisher
//        var user = new Users();
//        user.Id = new Guid;
//        user.Name = "Random4";
//        user.Phone = "123456";


//        context.UsersFromContext.Add(user);


//        // Saves changes
//        context.SaveChanges();
//    }
//}







