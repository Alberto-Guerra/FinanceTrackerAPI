using Microsoft.EntityFrameworkCore;
using FinanceTrackerAPI.Model;
using Microsoft.Extensions.Options;
using System.Configuration;
using FinanceTrackerAPI.Helper;

namespace FinanceTrackerAPI.Data
{
    public class DataContext : DbContext   
    {
        public DbSet<Transaction> Transactions => Set<Transaction>();
        public DbSet<Category> Categories => Set<Category>();
        public DbSet<User> Users => Set<User>();

        protected readonly IConfiguration Configuration;

        //create the needed constructor
        public DataContext(IConfiguration configuration)
        {
            Configuration = configuration;
        }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(ConnectionHelper.GetConnectionString(Configuration));
        }

    }
}
