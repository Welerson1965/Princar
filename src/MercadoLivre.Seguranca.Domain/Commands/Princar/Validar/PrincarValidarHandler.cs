using MediatR;
using MercadoLivre.Core.Domain.DTOs;
using MercadoLivre.Core.Domain.Interfaces.UoW;
using MercadoLivre.Seguranca.Domain.Commands.Princar.Notificacoes;
using MercadoLivre.Seguranca.Domain.Entities;
using MercadoLivre.Seguranca.Domain.Interfaces.MercadoLivre;
using MercadoLivre.Seguranca.Domain.Interfaces.Repositories;
using prmToolkit.NotificationPattern;
using System.Net.WebSockets;

namespace MercadoLivre.Seguranca.Domain.Commands.Princar.Validar
{
    public class PrincarValidarHandler : Notifiable, IRequestHandler<PrincarValidarRequest, CommandResponse>
    {
        private readonly IMercadoLivreApi _mercadoLivreApi;
        private readonly IUnitOfWork unitOfWork;
        private IRepositoryPedidoMercadoLivre _repositoryPedidoMercadoLivre;

        public PrincarValidarHandler(
            IMercadoLivreApi mercadoLivreApi,
            IRepositoryPedidoMercadoLivre repositoryPedidoMercadoLivre,
            IUnitOfWork unitOfWork)
        {
            _mercadoLivreApi = mercadoLivreApi;
            _repositoryPedidoMercadoLivre = repositoryPedidoMercadoLivre;
            this.unitOfWork = unitOfWork;
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

                //******************************************************//
                // Procedimentos para salvar ou atualizar o pedido
                //******************************************************//
                var nomeCliente = $"{pedido.buyer?.first_name ?? string.Empty} {pedido.buyer?.last_name ?? string.Empty}".Trim();
                var totalPedidoFormatado = pedido.total_amount.ToString("N2", new System.Globalization.CultureInfo("pt-BR"));
                var totalPagoFormatado = pedido.paid_amount.ToString("N2", new System.Globalization.CultureInfo("pt-BR"));
                var totalTaxasFormatado = pedido.taxes?.amount?.ToString("N2", new System.Globalization.CultureInfo("pt-BR")) ?? "0,00";

                var pedidoExiste = await _repositoryPedidoMercadoLivre.ExistsAsync(p => p.Id == pedidoId, cancellationToken);

                if (!pedidoExiste)
                {
                    // Criar um novo pedido na tabela PedidoMercadoLivre
                    var pedidoMercadoLivre = new PedidoMercadoLivre
                    {
                        Id = pedidoId,
                        EmpresaId = empresaId.ToString(),
                        DataCriacao = pedido.date_created,
                        DataAlteracao = pedido.last_updated,
                        DataFechamento = pedido.date_closed,
                        PackId = pedido.pack_id?.ToString(),
                        TotalPedido = Convert.ToDecimal(totalPedidoFormatado),
                        TotalPago = Convert.ToDecimal(totalPagoFormatado),
                        ShippingId = pedido.shipping?.id.ToString(),
                        Status = pedido.status,
                        VendedorId = pedido.seller?.id.ToString(),
                        ClienteId = pedido.buyer?.id.ToString(),
                        NickNameCliente = pedido.buyer?.nickname,
                        NomeCliente = nomeCliente,
                        TotalTaxas = string.IsNullOrEmpty(totalTaxasFormatado) ? null : Convert.ToDecimal(totalTaxasFormatado),
                    };

                    await _repositoryPedidoMercadoLivre.AddAsync(pedidoMercadoLivre, cancellationToken);
                }
                else
                {
                    // Atualizar o pedido existente na tabela PedidoMercadoLivre
                    var pedidoMercadoLivre = _repositoryPedidoMercadoLivre.ObterPorId(pedidoId);

                    if (pedidoMercadoLivre != null)
                    {
                        pedidoMercadoLivre.DataAlteracao = pedido.last_updated;
                        pedidoMercadoLivre.DataFechamento = pedido.date_closed;
                        pedidoMercadoLivre.PackId = pedido.pack_id?.ToString();
                        pedidoMercadoLivre.TotalPedido = Convert.ToDecimal(totalPedidoFormatado);
                        pedidoMercadoLivre.TotalPago = Convert.ToDecimal(totalPagoFormatado);
                        pedidoMercadoLivre.ShippingId = pedido.shipping?.id.ToString();
                        pedidoMercadoLivre.Status = pedido.status;
                        pedidoMercadoLivre.VendedorId = pedido.seller?.id.ToString();
                        pedidoMercadoLivre.ClienteId = pedido.buyer?.id.ToString();
                        pedidoMercadoLivre.NickNameCliente = pedido.buyer?.nickname;
                        pedidoMercadoLivre.NomeCliente = nomeCliente;
                        pedidoMercadoLivre.TotalTaxas = string.IsNullOrEmpty(totalTaxasFormatado) ? null : Convert.ToDecimal(totalTaxasFormatado);
                     
                        _repositoryPedidoMercadoLivre.Update(pedidoMercadoLivre);
                    }
                }

                unitOfWork.Commit();
            }

            return new CommandResponse(new PrincarValidarResponse("Pedido validado com sucesso"), this);
        }
    }
}
