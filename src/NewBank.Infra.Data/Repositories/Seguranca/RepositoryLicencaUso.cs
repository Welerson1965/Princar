using NewBank.Infra.Data.Context;
using NewBank.Infra.Data.Repositories.Base;
using NewBank.Seguranca.Domain.Entities.Seguranca;
using NewBank.Seguranca.Domain.Interfaces.Repositories;

namespace NewBank.Infra.Data.Repositories.Seguranca
{
    public class RepositoryLicencaUso : RepositoryBase<LicencaUsoEntity, Guid>, IRepositoryLicencaUso
    {
        public RepositoryLicencaUso(NewBankContext context) : base(context)
        {
        }

    }
}
