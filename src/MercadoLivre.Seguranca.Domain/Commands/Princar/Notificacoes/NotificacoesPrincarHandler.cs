using MediatR;
using MercadoLivre.Core.Domain.DTOs;
using MercadoLivre.Core.Domain.Interfaces.UoW;
using MercadoLivre.Seguranca.Domain.Commands.Princar.Validar;
using MercadoLivre.Seguranca.Domain.Entities;
using MercadoLivre.Seguranca.Domain.Interfaces.Repositories;
using prmToolkit.NotificationPattern;

namespace MercadoLivre.Seguranca.Domain.Commands.Princar.Notificacoes
{
    public class ProdutoHandler : Notifiable, IRequestHandler<NotificacoesPrincarRequest, CommandResponse>
    {
        private readonly IRepositoryPrincarNotificacoes _repositoryPrincarNotificacoes;
        private readonly IUnitOfWork unitOfWork;
        private readonly PrincarValidarHandler _princarValidarHandler;

        public ProdutoHandler(IRepositoryPrincarNotificacoes repositoryPrincarNotificacoes, IUnitOfWork unitOfWork)
        {
            _repositoryPrincarNotificacoes = repositoryPrincarNotificacoes;
            this.unitOfWork = unitOfWork;
        }

        public async Task<CommandResponse> Handle(NotificacoesPrincarRequest request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.resource) || string.IsNullOrEmpty(request.user_id))
            {
                AddNotification("Notificações", "Resource e User ID são obrigatórios.");
                return new CommandResponse(this);
            }

            var notificacao = new PrincarNotificacoesEntities(
                recurso: request.resource,
                usuarioId: request.user_id,
                topico: request.topic,
                aplicacaoId: request.application_id.ToString(),
                tentativas: request.attempts,
                dataEnvio: request.sent,
                dataRecebimento: request.received
            );

            await _repositoryPrincarNotificacoes.AddAsync(notificacao, cancellationToken);

            unitOfWork.Commit();

            PrincarValidarRequest validarRequest = new PrincarValidarRequest
            {
                Recurso = request.resource
            };

            var validarResponse = await _princarValidarHandler.Handle(validarRequest, cancellationToken);

            var response = new NotificacoesPrincarResponse("Notificação processada com sucesso");

            return new CommandResponse(response, this);
        }
    }
}
