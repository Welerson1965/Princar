using MercadoLivre.Infra.Data.Context;
using MercadoLivre.Infra.Data.Repositories.Base;
using MercadoLivre.Seguranca.Domain.Entities;
using MercadoLivre.Seguranca.Domain.Interfaces.Repositories;

namespace MercadoLivre.Infra.Data.Repositories.Seguranca
{
    public class RepositoryPedidoMercadoLivre : RepositoryBase<PedidoMercadoLivre, string>, IRepositoryPedidoMercadoLivre
    {
        public RepositoryPedidoMercadoLivre(MercadoLivreContext context) : base(context)
        {
        }
    }
}
