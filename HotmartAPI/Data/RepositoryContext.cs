using HotmartAPI.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace HotmartAPI.Data
{
    public class RepositoryContext : DbContext
    {
        protected RepositoryContext()
        {
        }
        public RepositoryContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Order> Orders { get; set; }
    }
}
