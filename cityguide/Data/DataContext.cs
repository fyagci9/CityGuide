using cityguide.Models;
using Microsoft.EntityFrameworkCore;

namespace cityguide.Data
{
    public class DataContext:DbContext
    {
        public DataContext(DbContextOptions<DataContext> options):base(options) 
        {
            
        }

        public DbSet<Value> Values { get; set; }
    }
}
