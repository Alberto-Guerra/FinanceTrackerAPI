using Microsoft.EntityFrameworkCore;
using FinanceTrackerAPI.Model;

namespace FinanceTrackerAPI.Data
{
    public class DataContext : DbContext   
    {
        public DbSet<Transaction> transactions => Set<Transaction>();
        public DbSet<Category> categories => Set<Category>();

        //create the needed constructor
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
          
        }

    }
}
