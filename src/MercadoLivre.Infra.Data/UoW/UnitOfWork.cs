using MercadoLivre.Core.Domain.DTOs;
using MercadoLivre.Core.Domain.Interfaces.UoW;
using MercadoLivre.Infra.Data.Context;

namespace MercadoLivre.Infra.Data.UoW
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly MercadoLivreContext _MercadoLivreContext;

        public UnitOfWork(MercadoLivreContext MercadoLivreContext)
        {
            _MercadoLivreContext = MercadoLivreContext;
        }

        public CommitResult Commit()
        {
            try
            {
                var x = _MercadoLivreContext.SaveChanges();
                return new CommitResult(true, null, null);
            }
            catch (Exception ex)
            {
                return new CommitResult(false, ex.Message, ex.InnerException?.Message);
            }
        }

    }
}
