using MercadoLivre.Infra.Data.Context;
using MercadoLivre.Infra.Data.Repositories.Base;
using MercadoLivre.Seguranca.Domain.Entities;
using MercadoLivre.Seguranca.Domain.Interfaces.Repositories;

namespace MercadoLivre.Infra.Data.Repositories.Seguranca
{
    public class RepositoryMercadoLivre : RepositoryBase<MercadoLivre.Seguranca.Domain.Entities.MercadoLivreParametro, Guid>, IRepositoryMercadoLivre
    {
        public RepositoryMercadoLivre(MercadoLivreContext context) : base(context)
        {
        }
    }
}
