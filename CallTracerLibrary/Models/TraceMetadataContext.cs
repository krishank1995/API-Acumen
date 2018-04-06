using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using MySql.Data.EntityFrameworkCore.Extensions;


namespace CallTracerLibrary.Models
{
    public class TraceMetadataContext : DbContext
    {
        public DbSet<TraceMetadata> CallTraces { get; set; }
        public TraceMetadataContext()
        {
        }
        public TraceMetadataContext(DbContextOptions options) : base(options) 
        {
        }

    }
}
