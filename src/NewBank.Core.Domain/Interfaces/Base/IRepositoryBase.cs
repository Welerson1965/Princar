using NewBank.Core.Domain.Entities.Base;
using System.Linq.Expressions;

namespace NewBank.Core.Domain.Interfaces.Base
{
    public interface IRepositoryBase<TEntity, TId>
        where TEntity : EntityBase<TId>, IAggregateRoot
    {
        #region Obter Por Id

        TEntity ObterPorId(TId id);
        TEntity ObterPorId(TId id, params Expression<Func<TEntity, object>>[] incluirPropriedadesNavegacao);
        TEntity ObterPorId(TId id, params string[] incluirPropriedadesNavegacao);

        #endregion
    }
}
