using MercadoLivre.Core.Domain.DTOs;

namespace MercadoLivre.Core.Domain.Interfaces.UoW
{
    public interface IUnitOfWork
    {
        CommitResult Commit();
    }
}
