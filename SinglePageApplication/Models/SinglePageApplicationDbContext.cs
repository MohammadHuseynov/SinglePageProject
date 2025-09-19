using Microsoft.EntityFrameworkCore;
using SinglePageApplication.Models.DomainModels.ProductAggregates;

namespace SinglePageApplication.Models
{
    public class SinglePageApplicationDbContext : DbContext
    {
        public SinglePageApplicationDbContext()
        {

        }

        public SinglePageApplicationDbContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<Product> Product { get; set; }

    }

}
