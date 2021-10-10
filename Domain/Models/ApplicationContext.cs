namespace Domain.Models
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    public class DomainContext : IdentityDbContext
    {
        public DbSet<Crypto> Cryptos { get; set; }
        public DbSet<Metadata> Metadata { get; set; }

        public DomainContext(DbContextOptions<DomainContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
