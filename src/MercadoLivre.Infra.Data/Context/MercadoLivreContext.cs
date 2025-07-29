using MercadoLivre.Seguranca.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using prmToolkit.NotificationPattern;

namespace MercadoLivre.Infra.Data.Context
{
    public class MercadoLivreContext : DbContext
    {
        public MercadoLivreContext() { }

        public MercadoLivreContext(DbContextOptions<MercadoLivreContext> options) : base(options) { }

        public DbSet<PrincarNotificacoesEntities> PrincarNotificacoes { get; set; }
        public DbSet<MercadoLivreParametro> MercadoLivreParametro { get; set; }
        public DbSet<PedidoMercadoLivre> PedidoMercadoLivre { get; set; }
        public DbSet<PedidoItemMercadoLivre> PedidoItemMercadoLivre { get; set; }
        public DbSet<PedidoPgtoMercadoLivre> PedidoPgtoMercadoLivre { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(MercadoLivreContext).Assembly);

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
