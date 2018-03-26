using Microsoft.EntityFrameworkCore;
using MySql.Data.EntityFrameworkCore.Extensions;

namespace CallTracerLibrary.Models
{
    //[DbConfigurationType(typeof(MySqlEFConfiguration))]
    public class UsersContext:DbContext
    {
       public DbSet<Users> UsersFromContext { get; set; }



    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
      optionsBuilder.UseMySQL("server=localhost;database=library;user=root;password=password;SslMode=none;");
    }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);



            modelBuilder.Entity<Users>(entity =>
            {
                 entity.HasKey(e => e.Id);
                //entity.Property(e => e.Name).IsRequired();

            });
        }



    }


}
