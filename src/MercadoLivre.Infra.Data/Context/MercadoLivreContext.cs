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
        public DbSet<MercadoLivreEntities> MercadoLivreEntities { get; set; }
        public DbSet<PedidoMercadoLivreEntities> pedidoMercadoLivreEntities { get; set; }
        public DbSet<PedidoItemMercadoLivreEntities> pedidoItemMercadoLivreEntities { get; set; }
        public DbSet<PedidoPgtoMercadoLivreEntities> pedidoPgtoMercadoLivreEntities { get; set; }

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
