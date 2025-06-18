using Princar.Infra.Data.Context;
using Princar.Infra.Data.Repositories.Base;
using Princar.Seguranca.Domain.Entities.Produto;
using Princar.Seguranca.Domain.Interfaces.Repositories;

namespace Princar.Infra.Data.Repositories.Seguranca
{
    public class RepositoryProduto : RepositoryBase<ProdutoEntity, int>, IRepositoryProduto
    {
        public RepositoryProduto(PrincarContext context) : base(context)
        {
        }
    }
}
