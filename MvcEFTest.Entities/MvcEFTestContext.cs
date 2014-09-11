using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace MvcEFTest.Entities
{
    public class MvcEFTestContext : DbContext
    {
        public DbSet<Manufacturer> Manufacturers { get; set; }

        public DbSet<Phone> Phones { get; set; }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}
