using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using BankAPIApplication.Entities;

namespace BankAPIApplication.Helpers
{
    public class DataContext : DbContext
    {
        protected readonly IConfiguration Configuration;

        public DataContext(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            // connect to sql server database
            options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Transaction>()
                .Property(b => b.CreatedAt)
                .HasDefaultValueSql("GETDATE()");
            modelBuilder.Entity<Transaction>()
               .Property(b => b.UpdatedAt)
               .HasDefaultValueSql("GETDATE()");
            modelBuilder.Entity<User>()
                .Property(b => b.CreatedAt)
                .HasDefaultValueSql("GETDATE()");
            modelBuilder.Entity<User>()
                .Property(b => b.UpdatedAt)
                .HasDefaultValueSql("GETDATE()");
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
    }
}
