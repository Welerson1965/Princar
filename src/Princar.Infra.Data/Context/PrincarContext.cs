using Microsoft.EntityFrameworkCore;
using Princar.Seguranca.Domain.Entities.Seguranca;
using prmToolkit.NotificationPattern;

namespace Princar.Infra.Data.Context
{
    public class PrincarContext : DbContext
    {
        public PrincarContext() { }

        public PrincarContext(DbContextOptions<PrincarContext> options) : base(options) { }

        public DbSet<LicencaUsoEntity> LicencaUso { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(PrincarContext).Assembly);

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
