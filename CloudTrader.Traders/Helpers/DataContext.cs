using CloudtraderTraders.Models;
using Microsoft.EntityFrameworkCore;

namespace CloudtraderTraders.Helpers
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        {
        }

        public DbSet<TraderModel> Traders { get; set; }
    }
}
