using Microsoft.EntityFrameworkCore;


namespace Mail.Domain
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Mail.Domain.Entities.User> Users { get; set; } = default!;
        public DbSet<Mail.Domain.Entities.Message> Messages { get; set; } = default!;

        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSnakeCaseNamingConvention();
        }
    }
}
