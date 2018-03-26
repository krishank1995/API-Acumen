using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using MySql.Data.EntityFrameworkCore.Extensions;


namespace CallTracerLibrary.Models
{
    //class TraceMetadataContext : DbContext
    //{
    //    public DbSet<TraceMetadata> CallTrace { get; set; }

    //    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //    {
    //        optionsBuilder.UseMySQL("server=localhost;database=ApiCallTraces;user=root;password=password;SslMode=none;");
    //    }

    //    protected override void OnModelCreating(ModelBuilder modelBuilder)
    //    {
    //        base.OnModelCreating(modelBuilder);



    //        modelBuilder.Entity<TraceMetadata>(entity =>
    //        {
    //            entity.HasKey(e => e.Id);
    //            //entity.Property(e => e.Name).IsRequired();

    //        });
    //    }
    //}
}
