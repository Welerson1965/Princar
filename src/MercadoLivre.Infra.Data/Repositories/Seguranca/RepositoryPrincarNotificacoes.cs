using MercadoLivre.Infra.Data.Context;
using MercadoLivre.Infra.Data.Repositories.Base;
using MercadoLivre.Seguranca.Domain.Entities;
using MercadoLivre.Seguranca.Domain.Interfaces.Repositories;

namespace MercadoLivre.Infra.Data.Repositories.Seguranca
{
    public class RepositoryPrincarNotificacoes : RepositoryBase<PrincarNotificacoesEntities, string>, IRepositoryPrincarNotificacoes
    {
        public RepositoryPrincarNotificacoes(MercadoLivreContext context) : base(context)
        {
        }
    }
}
