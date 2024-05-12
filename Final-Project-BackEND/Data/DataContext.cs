using Final_Project_BackEND.Entity;
using Microsoft.EntityFrameworkCore;

namespace Final_Project_BackEND.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) 
        {
            
        }

        public DbSet<User> Users { get; set; }
        public DbSet<ImportHeader> importHeaders { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Excel> Excels { get; set; }
        public DbSet<GradeImportHeader> GradeImportHeaders { get; set; }
        public DbSet<GradeFilter> GradeFilter { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<OrderVendor> OrderVendors { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<RaweeTest> RaweeTest { get; set; }

        internal object ExecuteQuery(string v)
        {
            throw new NotImplementedException();
        }
    }
}
