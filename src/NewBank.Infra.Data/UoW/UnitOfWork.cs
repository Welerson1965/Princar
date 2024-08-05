using NewBank.Core.Domain.DTOs;
using NewBank.Core.Domain.Interfaces.UoW;
using NewBank.Infra.Data.Context;

namespace NewBank.Infra.Data.UoW
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly NewBankContext _newBankContext;

        public UnitOfWork(NewBankContext newBankContext)
        {
            _newBankContext = newBankContext;
        }

        public CommitResult Commit()
        {
            try
            {
                var x = _newBankContext.SaveChanges();
                return new CommitResult(true, null, null);
            }
            catch (Exception ex)
            {
                return new CommitResult(false, ex.Message, ex.InnerException?.Message);
            }
        }

    }
}
