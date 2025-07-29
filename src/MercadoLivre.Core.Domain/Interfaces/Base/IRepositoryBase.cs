using MercadoLivre.Core.Domain.Entities.Base;
using System.Linq.Expressions;

namespace MercadoLivre.Core.Domain.Interfaces.Base
{
    public interface IRepositoryBase<TEntity, TId>
        where TEntity : EntityBase<TId>, IAggregateRoot
    {

        #region Exists

        Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> where, CancellationToken cancellationToken = default);

        #endregion

        #region Obter Por Id

        TEntity ObterPorId(TId id);
        TEntity ObterPorId(TId id, params Expression<Func<TEntity, object>>[] incluirPropriedadesNavegacao);
        TEntity ObterPorId(TId id, params string[] incluirPropriedadesNavegacao);

        #endregion
    
        #region Listar Por Sem Rastreamento...

        IEnumerable<TEntity> ListarPorSemRastreamento(Func<TEntity, bool> onde);
        IEnumerable<TEntity> ListarPorSemRastreamento(Func<TEntity, bool> onde, Func<TEntity, object> ordem, bool ascendente);
        IEnumerable<TEntity> ListarPorSemRastreamento(Func<TEntity, bool> onde, Func<TEntity, object> ordem, bool ascendente, params Expression<Func<TEntity, object>>[] incluirPropriedadesNavegacao);
        IEnumerable<TEntity> ListarPorSemRastreamento(Func<TEntity, bool> onde, Func<TEntity, object> ordem, bool ascendente, params string[] incluirPropriedadesNavegacao);
        IEnumerable<TEntity> ListarPorSemRastreamento(Func<TEntity, bool> onde, params Expression<Func<TEntity, object>>[] incluirPropriedadesNavegacao);
        IEnumerable<TEntity> ListarPorSemRastreamento(Func<TEntity, bool> onde, params string[] incluirPropriedadesNavegacao);

        #endregion

        #region Add

        Task AddAsync(TEntity entity, CancellationToken cancellationToken = default);
        Task AddCollectionAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);

        #endregion

        #region Update

        void Update(TEntity entity);
        void UpdateCollection(IEnumerable<TEntity> entities);

        #endregion
    }
}