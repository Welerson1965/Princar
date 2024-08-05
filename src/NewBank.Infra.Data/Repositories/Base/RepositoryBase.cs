using Microsoft.EntityFrameworkCore;
using NewBank.Core.Domain.Entities.Base;
using NewBank.Core.Domain.Interfaces.Base;
using NewBank.Core.Domain.Interfaces;
using System.Linq.Expressions;
using NewBank.Infra.Data.Context;

namespace NewBank.Infra.Data.Repositories.Base
{
    public class RepositoryBase<TEntity, TId> : IRepositoryBase<TEntity, TId>
            where TEntity : EntityBase<TId>, IAggregateRoot
    {
        protected readonly DbSet<TEntity> DbSet;
        protected readonly NewBankContext Context;

        public RepositoryBase(NewBankContext context)
        {
            Context = context;
            DbSet = Context.Set<TEntity>();
        }

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
    }
}
