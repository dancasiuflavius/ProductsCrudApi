

using Microsoft.EntityFrameworkCore;
using ProductsCrudApi.Products.Model;

namespace ProductsCrudApi.Data
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions <AppDbContext> options) : base(options)
        {

        }
        public virtual DbSet<Product> Products { get; set; }
    }   
}
