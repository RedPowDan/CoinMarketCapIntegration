namespace Domain.Models
{
    using Microsoft.EntityFrameworkCore;
    public class DomainContext : DbContext
    {
        public DbSet<Crypto> Cryptos { get; set; }

        public DomainContext(DbContextOptions<DomainContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
