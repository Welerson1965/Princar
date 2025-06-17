using Princar.Core.Domain.DTOs;
using Princar.Core.Domain.Interfaces.UoW;
using Princar.Infra.Data.Context;

namespace Princar.Infra.Data.UoW
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly PrincarContext _princarContext;

        public UnitOfWork(PrincarContext princarContext)
        {
            _princarContext = princarContext;
        }

        public CommitResult Commit()
        {
            try
            {
                var x = _princarContext.SaveChanges();
                return new CommitResult(true, null, null);
            }
            catch (Exception ex)
            {
                return new CommitResult(false, ex.Message, ex.InnerException?.Message);
            }
        }

    }
}
