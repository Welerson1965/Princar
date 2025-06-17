using Princar.Core.Domain.DTOs;

namespace Princar.Core.Domain.Interfaces.UoW
{
    public interface IUnitOfWork
    {
        CommitResult Commit();
    }
}
