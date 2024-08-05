using NewBank.Core.Domain.DTOs;

namespace NewBank.Core.Domain.Interfaces.UoW
{
    public interface IUnitOfWork
    {
        CommitResult Commit();
    }
}
