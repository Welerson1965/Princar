using Microsoft.EntityFrameworkCore;
using MercadoLivre.Core.Domain.Entities.Base;
using MercadoLivre.Core.Domain.Interfaces.Base;
using MercadoLivre.Core.Domain.Interfaces;
using System.Linq.Expressions;
using MercadoLivre.Infra.Data.Context;

namespace MercadoLivre.Infra.Data.Repositories.Base
{
    public class RepositoryBase<TEntity, TId> : IRepositoryBase<TEntity, TId>
            where TEntity : EntityBase<TId>, IAggregateRoot
    {
        protected readonly DbSet<TEntity> DbSet;
        protected readonly MercadoLivreContext Context;

        public RepositoryBase(MercadoLivreContext context)
        {
            Context = context;
            DbSet = Context.Set<TEntity>();
        }

        #region Exists

        public async Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> where,
            CancellationToken cancellationToken = default)
        {
            return await DbSet.AsNoTracking().AnyAsync(where, cancellationToken);
        }

        #endregion

        public TEntity ObterPorId(TId id)
        {
            return DbSet.Find(id);
        }

        public TEntity ObterPorId(TId id, params Expression<Func<TEntity, object>>[] incluirPropriedadesNavegacao)
        {
            if (incluirPropriedadesNavegacao.Any())
                return Include(DbSet, incluirPropriedadesNavegacao).FirstOrDefault(p => p.Id.Equals(id));

            return DbSet.FirstOrDefault(p => p.Id.Equals(id));
        }

        public TEntity ObterPorId(TId id, params string[] incluirPropriedadesNavegacao)
        {
            if (incluirPropriedadesNavegacao.Any())
                return Include(DbSet, incluirPropriedadesNavegacao).FirstOrDefault(p => p.Id.Equals(id));

            return DbSet.FirstOrDefault(p => p.Id.Equals(id));
        }

        private IQueryable<TEntity> Include(IQueryable<TEntity> query, params Expression<Func<TEntity, object>>[] incluirPropriedadesNavegacao)
        {
            foreach (var property in incluirPropriedadesNavegacao)
                query = query.Include(property);

            return query;
        }

        private IQueryable<TEntity> Include(IQueryable<TEntity> query, params string[] incluirPropriedadesNavegacao)
        {
            foreach (var property in incluirPropriedadesNavegacao)
                query = query.Include(property);

            return query;
        }
        #region Listar Por Sem Rastreamento...

        public IEnumerable<TEntity> ListarPorSemRastreamento(Func<TEntity, bool> onde)
        {
            return DbSet.AsNoTracking().Where(onde).AsEnumerable();
        }

        public IEnumerable<TEntity> ListarPorSemRastreamento(Func<TEntity, bool> onde, Func<TEntity, object> ordem, bool ascendente)
        {
            return ascendente
                ? DbSet.AsNoTracking().Where(onde).OrderBy(ordem).AsEnumerable()
                : DbSet.AsNoTracking().Where(onde).OrderByDescending(ordem).AsEnumerable();
        }

        public IEnumerable<TEntity> ListarPorSemRastreamento(Func<TEntity, bool> onde, Func<TEntity, object> ordem, bool ascendente, params Expression<Func<TEntity, object>>[] incluirPropriedadesNavegacao)
        {
            if (incluirPropriedadesNavegacao.Any())
                return ascendente
                    ? Include(DbSet, incluirPropriedadesNavegacao).AsNoTracking().Where(onde).OrderBy(ordem).AsEnumerable()
                    : Include(DbSet, incluirPropriedadesNavegacao).AsNoTracking().Where(onde).OrderByDescending(ordem).AsEnumerable();

            return ascendente
                ? DbSet.AsNoTracking().Where(onde).OrderBy(ordem).AsEnumerable()
                : DbSet.AsNoTracking().Where(onde).OrderByDescending(ordem).AsEnumerable();
        }

        public IEnumerable<TEntity> ListarPorSemRastreamento(Func<TEntity, bool> onde, Func<TEntity, object> ordem, bool ascendente, params string[] incluirPropriedadesNavegacao)
        {
            if (incluirPropriedadesNavegacao.Any())
                return ascendente
                    ? Include(DbSet, incluirPropriedadesNavegacao).AsNoTracking().Where(onde).OrderBy(ordem).AsEnumerable()
                    : Include(DbSet, incluirPropriedadesNavegacao).AsNoTracking().Where(onde).OrderByDescending(ordem).AsEnumerable();

            return ascendente
                ? DbSet.AsNoTracking().Where(onde).OrderBy(ordem).AsEnumerable()
                : DbSet.AsNoTracking().Where(onde).OrderByDescending(ordem).AsEnumerable();
        }

        public IEnumerable<TEntity> ListarPorSemRastreamento(Func<TEntity, bool> onde, params Expression<Func<TEntity, object>>[] incluirPropriedadesNavegacao)
        {
            if (incluirPropriedadesNavegacao.Any())
                return Include(DbSet, incluirPropriedadesNavegacao).AsNoTracking().Where(onde).AsEnumerable();

            return DbSet.AsNoTracking().Where(onde).AsEnumerable();
        }

        public IEnumerable<TEntity> ListarPorSemRastreamento(Func<TEntity, bool> onde, params string[] incluirPropriedadesNavegacao)
        {
            if (incluirPropriedadesNavegacao.Any())
                return Include(DbSet, incluirPropriedadesNavegacao).AsNoTracking().Where(onde).AsEnumerable();

            return DbSet.AsNoTracking().Where(onde).AsEnumerable();
        }

        #endregion

        #region Add

        public async Task AddAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            await DbSet.AddAsync(entity, cancellationToken);
        }

        public async Task AddCollectionAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
        {
            await DbSet.AddRangeAsync(entities, cancellationToken);
        }

        #endregion

        #region Update

        public void Update(TEntity entity)
        {
            DbSet.Update(entity);
        }

        public void UpdateCollection(IEnumerable<TEntity> entities)
        {
            DbSet.UpdateRange(entities);
        }

        #endregion

        #region Count

        public async Task<int> CountAsync(CancellationToken cancellationToken = default)
        {
            return await DbSet.AsNoTracking().CountAsync(cancellationToken);
        }

        public async Task<int> CountAsync(Expression<Func<TEntity, bool>> where,
            CancellationToken cancellationToken = default)
        {
            return await DbSet.AsNoTracking().CountAsync(where, cancellationToken);
        }

        #endregion

        #region GetBy

        public async Task<TEntity?> GetByAsync(bool tracking, Expression<Func<TEntity, bool>> where,
            CancellationToken cancellationToken = default)
        {
            return tracking
                ? await DbSet.FirstOrDefaultAsync(where, cancellationToken)
                : await DbSet.AsNoTracking().FirstOrDefaultAsync(where, cancellationToken);
        }

        public async Task<TEntity?> GetByAsync(bool tracking, Expression<Func<TEntity, bool>> where,
            CancellationToken cancellationToken = default, params Expression<Func<TEntity, object>>[] navigationProperties)
        {
            var query = DbSet.AsQueryable();
            query = Include(query, navigationProperties);
            return tracking
                ? await query.FirstOrDefaultAsync(where, cancellationToken)
                : await query.AsNoTracking().FirstOrDefaultAsync(where, cancellationToken);
        }

        public async Task<TEntity?> GetByAsync(bool tracking, Expression<Func<TEntity, bool>> where,
            CancellationToken cancellationToken = default, params string[] navigationProperties)
        {
            var query = DbSet.AsQueryable();
            query = Include(query, navigationProperties);
            return tracking
                ? await query.FirstOrDefaultAsync(where, cancellationToken)
                : await query.AsNoTracking().FirstOrDefaultAsync(where, cancellationToken);
        }

        #endregion

        #region Delete

        public void DeleteAsync(TEntity entity)
        {
            DbSet.Remove(entity);
        }

        public void DeleteCollectionAsync(IEnumerable<TEntity> entities)
        {
            DbSet.RemoveRange(entities);
        }

        #endregion
    }
}
