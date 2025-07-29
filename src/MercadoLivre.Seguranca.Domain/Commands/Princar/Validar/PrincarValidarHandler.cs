using MediatR;
using MercadoLivre.Core.Domain.DTOs;
using MercadoLivre.Seguranca.Domain.Commands.Princar.Notificacoes;
using MercadoLivre.Seguranca.Domain.Interfaces.MercadoLivre;
using prmToolkit.NotificationPattern;

namespace MercadoLivre.Seguranca.Domain.Commands.Princar.Validar
{
    public class PrincarValidarHandler : Notifiable, IRequestHandler<PrincarValidarRequest, CommandResponse>
    {
        private readonly IMercadoLivreApi _mercadoLivreApi;

        public PrincarValidarHandler(IMercadoLivreApi mercadoLivreApi)
        {
            _mercadoLivreApi = mercadoLivreApi;
        }

        public async Task<CommandResponse> Handle(PrincarValidarRequest request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.Recurso))
            {
                AddNotification("PrincarValidar", "O recurso é obrigatório.");
                return new CommandResponse(this);
            }

            //******************************************************//
            // Verificar se a notificação é de Pedidos
            //******************************************************//
            if (request.Recurso.StartsWith("/orders/"))
            {
                // Validar Token
                Guid empresaId = Guid.Parse("23bacfdc-e577-4acb-b5f3-964547dec026");

                var token = _mercadoLivreApi.BuscarToken(empresaId);

                // Extrair o ID do pedido do recurso
                var pedidoId = request.Recurso.Split('/')[2];
                
                // Buscar o pedido individual usando o ID
                var pedido = _mercadoLivreApi.BuscarPedidoIndividual(pedidoId, token.access_token);
                
                if (pedido == null)
                {
                    AddNotification("PrincarValidar", "Pedido não encontrado.");
                    return new CommandResponse(this);
                }
            }



            return new CommandResponse(new PrincarValidarResponse("Pedido validado com sucesso"), this);
        }
    }
}
