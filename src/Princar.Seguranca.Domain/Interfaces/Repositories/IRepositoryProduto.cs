using Princar.Core.Domain.Interfaces.Base;
using Princar.Seguranca.Domain.Entities.Produto;

namespace Princar.Seguranca.Domain.Interfaces.Repositories
{
    public interface IRepositoryProduto : IRepositoryBase<ProdutoEntity, Guid>
    {
    }
}
