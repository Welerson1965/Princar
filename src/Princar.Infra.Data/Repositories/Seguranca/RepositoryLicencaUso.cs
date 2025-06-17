using Princar.Infra.Data.Context;
using Princar.Infra.Data.Repositories.Base;
using Princar.Seguranca.Domain.Entities.Seguranca;
using Princar.Seguranca.Domain.Interfaces.Repositories;

namespace Princar.Infra.Data.Repositories.Seguranca
{
    public class RepositoryLicencaUso : RepositoryBase<LicencaUsoEntity, Guid>, IRepositoryLicencaUso
    {
        public RepositoryLicencaUso(PrincarContext context) : base(context)
        {
        }
    }
}
