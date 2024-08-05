using Microsoft.EntityFrameworkCore;
using NewBank.Seguranca.Domain.Entities.Seguranca;
using prmToolkit.NotificationPattern;

namespace NewBank.Infra.Data.Context
{
    public class NewBankContext : DbContext
    {
        public NewBankContext() { }

        public NewBankContext(DbContextOptions<NewBankContext> options) : base(options) { }

        public DbSet<LicencaUsoEntity> LicencaUso { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(NewBankContext).Assembly);

            modelBuilder.Ignore<Notification>();

            base.OnModelCreating(modelBuilder);
        }

        public override int SaveChanges()
        {
            var addedEntities = ChangeTracker.Entries()
                .Where(p => p.State == EntityState.Added).ToList();

            var modifiedEntities = ChangeTracker.Entries()
                .Where(p => p.State == EntityState.Modified).ToList();

            return base.SaveChanges();
        }
    }
}
