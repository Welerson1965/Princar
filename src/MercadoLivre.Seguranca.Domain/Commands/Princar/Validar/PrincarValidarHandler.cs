﻿using MediatR;
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
        private IRepositoryPedidoItemMercadoLivre _repositoryPedidoItemMercadoLivre;
        private IRepositoryPedidoPgtoMercadoLivre _repositoryPedidoPgtoMercadoLivre;

        public PrincarValidarHandler(
            IMercadoLivreApi mercadoLivreApi,
            IRepositoryPedidoMercadoLivre repositoryPedidoMercadoLivre,
            IRepositoryPedidoItemMercadoLivre repositoryPedidoItemMercadoLivre,
            IRepositoryPedidoPgtoMercadoLivre repositoryPedidoPgtoMercadoLivre,
            IUnitOfWork unitOfWork)
        {
            _mercadoLivreApi = mercadoLivreApi;
            _repositoryPedidoMercadoLivre = repositoryPedidoMercadoLivre;
            _repositoryPedidoItemMercadoLivre = repositoryPedidoItemMercadoLivre;
            _repositoryPedidoPgtoMercadoLivre = repositoryPedidoPgtoMercadoLivre;
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

                //*******************************************************//
                // Procedimento para salvar ou atualizar itens do pedido
                //*******************************************************//
                if (pedido.order_items != null )
                {
                    foreach (var item in pedido.order_items)
                    {
                        var guid = Guid.NewGuid();
                        var qtdeFormatada = item.quantity.ToString("N6", new System.Globalization.CultureInfo("pt-BR"));
                        var valorunitFormatado = item.unit_price.ToString("N2", new System.Globalization.CultureInfo("pt-BR"));
                        var valorfullFormatado = item.full_unit_price.ToString("N2", new System.Globalization.CultureInfo("pt-BR"));
                        var taxaVendaFormatada = item.sale_fee.ToString("N2", new System.Globalization.CultureInfo("pt-BR")) ?? "0,00";

                        // Verifica se o item já existe
                        var itemExiste = await _repositoryPedidoItemMercadoLivre.ExistsAsync(
                            p => p.PedidoId == pedidoId &&
                                 p.ItemId == item.item.id, cancellationToken);
                        if (!itemExiste)
                        {
                            // Criar um novo item do pedido na tabela PedidoItemMercadoLivre
                            var pedidoItem = new PedidoItemMercadoLivre
                            {
                                Id = guid,
                                PedidoId = pedidoId,
                                ItemId = item.item.id,
                                Descricao = item.item.title,
                                CategoriaId = item.item.category_id,
                                SKU = item.item.seller_sku,
                                Quantidade = Convert.ToDecimal(qtdeFormatada),
                                ValorUnitario = Convert.ToDecimal(valorunitFormatado),
                                ValorFullUnitario = Convert.ToDecimal(valorfullFormatado),
                                Moeda = item.currency_id,
                                TaxaVenda = string.IsNullOrEmpty(taxaVendaFormatada) ? null : Convert.ToDecimal(taxaVendaFormatada),
                            };

                            await _repositoryPedidoItemMercadoLivre.AddAsync(pedidoItem, cancellationToken);
                        }
                        else
                        {
                            // Atualizar o item do pedido existente na tabela PedidoItemMercadoLivre
                            var pedidoItem = _repositoryPedidoItemMercadoLivre.GetByAsync(
                                false, 
                                p => p.PedidoId == pedidoId &&
                                     p.ItemId == item.item.id, cancellationToken);
                            if (pedidoItem != null)
                            {
                                var pedidoItemMercadoLivre = pedidoItem.Result;

                                pedidoItemMercadoLivre.Descricao = item.item.title;
                                pedidoItemMercadoLivre.CategoriaId = item.item.category_id;
                                pedidoItemMercadoLivre.SKU = item.item.seller_sku;
                                pedidoItemMercadoLivre.Quantidade = Convert.ToDecimal(qtdeFormatada);
                                pedidoItemMercadoLivre.ValorUnitario = Convert.ToDecimal(valorunitFormatado);
                                pedidoItemMercadoLivre.ValorFullUnitario = Convert.ToDecimal(valorfullFormatado);
                                pedidoItemMercadoLivre.Moeda = item.currency_id;
                                pedidoItemMercadoLivre.TaxaVenda = string.IsNullOrEmpty(taxaVendaFormatada) ? null : Convert.ToDecimal(taxaVendaFormatada);

                                _repositoryPedidoItemMercadoLivre.Update(pedidoItemMercadoLivre);
                            }
                        }
                    }
                }

                //*******************************************************//
                // Procedimento para salvar ou atualizar Pagto do pedido
                //*******************************************************//
                if (pedido.payments != null)
                {
                    foreach (var pagamento in pedido.payments)
                    {
                        var guidPagamento = Guid.NewGuid();
                        var valorPagtoFormatado = pagamento.transaction_amount?.ToString("N2", new System.Globalization.CultureInfo("pt-BR"));
                        var valorDevolvidaFormatado = pagamento.transaction_amount_refunded?.ToString("N2", new System.Globalization.CultureInfo("pt-BR")) ?? "0,00";
                        var valorTaxasFormatado = pagamento.taxes_amount?.ToString("N2", new System.Globalization.CultureInfo("pt-BR")) ?? "0,00";
                        var valorCupomFormatado = pagamento.coupon_amount?.ToString("N2", new System.Globalization.CultureInfo("pt-BR")) ?? "0,00";
                        var valorAcrescimoFormatado = pagamento.overpaid_amount?.ToString("N2", new System.Globalization.CultureInfo("pt-BR")) ?? "0,00";
                        var totalPagamentoFormatado = pagamento.total_paid_amount?.ToString("N2", new System.Globalization.CultureInfo("pt-BR")) ?? "0,00";
                        var taxaMarketPlaceFormatada = pagamento.marketplace_fee.ToString("N2", new System.Globalization.CultureInfo("pt-BR")) ?? "0,00";

                        var pagamentoExiste = await _repositoryPedidoPgtoMercadoLivre.ExistsAsync(
                            p => p.PedidoId == pedidoId &&
                                 p.IdML == pagamento.id, cancellationToken);
                        if (!pagamentoExiste)
                        {
                            // Criar um novo pagamento do pedido na tabela PedidoPgtoMercadoLivre
                            var pedidoPgtoMercadoLivre = new PedidoPgtoMercadoLivre
                            {
                                Id = guidPagamento,
                                PedidoId = pedidoId,
                                IdML = pagamento.id,
                                PagamentoId = pagamento.payer_id.ToString(),
                                ColetorId = pagamento.collector.id.ToString(),
                                CartaoId = pagamento.card_id?.ToString(),
                                SiteId = pagamento.site_id,
                                TipoPagamento = pagamento.payment_method_id,
                                Status = pagamento.status,
                                StatusDetalhe = pagamento.status_detail,
                                ValorPagto = Convert.ToDecimal(valorPagtoFormatado),
                                ValorDevolvida = Convert.ToDecimal(valorDevolvidaFormatado),
                                ValorTaxas = Convert.ToDecimal(valorTaxasFormatado),
                                ValorCupom = Convert.ToDecimal(valorCupomFormatado),
                                ValorAcrescimo = Convert.ToDecimal(valorAcrescimoFormatado),
                                TotalPagamento = Convert.ToDecimal(totalPagamentoFormatado),
                                TaxaMarketPlace = Convert.ToDecimal(taxaMarketPlaceFormatada),
                                DataAprovacao = pagamento.date_approved,
                                DataCriacao = pagamento.date_created,
                                DataModificacao = pagamento.date_last_modified
                            };

                            await _repositoryPedidoPgtoMercadoLivre.AddAsync(pedidoPgtoMercadoLivre, cancellationToken);
                        }
                        else
                        {
                            // Atualizar o pagamento do pedido existente na tabela PedidoPgtoMercadoLivre
                            var pedidoPgtoMercadoLivre = _repositoryPedidoPgtoMercadoLivre.GetByAsync(
                                false, 
                                p => p.PedidoId == pedidoId &&
                                     p.IdML == pagamento.id, cancellationToken);
                            if (pedidoPgtoMercadoLivre != null)
                            {
                                var pgtoMercadoLivre = pedidoPgtoMercadoLivre.Result;

                                pgtoMercadoLivre.DataAprovacao = pagamento.date_approved;
                                pgtoMercadoLivre.DataCriacao = pagamento.date_created;
                                pgtoMercadoLivre.DataModificacao = pagamento.date_last_modified;
                                pgtoMercadoLivre.ValorPagto = Convert.ToDecimal(valorPagtoFormatado);
                                pgtoMercadoLivre.ValorDevolvida = Convert.ToDecimal(valorDevolvidaFormatado);
                                pgtoMercadoLivre.ValorTaxas = Convert.ToDecimal(valorTaxasFormatado);
                                pgtoMercadoLivre.ValorCupom = Convert.ToDecimal(valorCupomFormatado);
                                pgtoMercadoLivre.ValorAcrescimo = Convert.ToDecimal(valorAcrescimoFormatado);
                                pgtoMercadoLivre.TotalPagamento = Convert.ToDecimal(totalPagamentoFormatado);
                                pgtoMercadoLivre.TaxaMarketPlace = Convert.ToDecimal(taxaMarketPlaceFormatada);

                                _repositoryPedidoPgtoMercadoLivre.Update(pgtoMercadoLivre);
                            }
                        }
                    }
                }

                unitOfWork.Commit();
            }

            return new CommandResponse(new PrincarValidarResponse("Pedido validado com sucesso"), this);
        }
    }
}
