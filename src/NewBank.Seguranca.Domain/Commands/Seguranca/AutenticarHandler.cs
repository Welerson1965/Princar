using MediatR;
using NewBank.Core.Domain.DTOs;
using NewBank.Seguranca.Domain.Enumerators.Seguranca;
using NewBank.Seguranca.Domain.Interfaces.Repositories;
using prmToolkit.NotificationPattern;

namespace NewBank.Seguranca.Domain.Commands.Seguranca
{
    public class AutenticarHandler : Notifiable, IRequestHandler<AutenticarRequest, CommandResponse>
    {
        private readonly IRepositoryLicencaUso _repositoryLicencaUso;

        public AutenticarHandler(IRepositoryLicencaUso repositoryLicencaUso)
        {
            _repositoryLicencaUso = repositoryLicencaUso;
        }

        public Task<CommandResponse> Handle(AutenticarRequest request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                AddNotification("Autenticação", "Código da autenticação não enviada");
                return Task.FromResult(new CommandResponse(this));
            }

            Guid guid = Guid.Parse(request.KeyId);

            var licencaUso = _repositoryLicencaUso.ObterPorId(guid);

            if (licencaUso == null)
            {
                AddNotification("Autenticação", "Código da autenticação não localizado");
                return Task.FromResult(new CommandResponse(this));
            }

            if (licencaUso.FlagLicencaUso == FlagLicencaUso.Inativo)
            {
                AddNotification("Autenticação", "Licença de uso está inativa");
                return Task.FromResult(new CommandResponse(this));
            }

            if (licencaUso.FlagLicencaUso == FlagLicencaUso.Bloqueado)
            {
                AddNotification("Autenticação", "Licença de uso está bloqueada");
                return Task.FromResult(new CommandResponse(this));
            }

            return Task.FromResult(new CommandResponse(new AutenticarResponse
            {
                NomeEmpresa = licencaUso.NomeEmpresa
            }, this));
        }
    }
}
