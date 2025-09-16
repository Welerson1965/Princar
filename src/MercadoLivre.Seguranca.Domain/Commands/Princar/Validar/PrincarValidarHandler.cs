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
        private IRepositoryPedidoItemMercadoLivre _repositoryPedidoItemMercadoLivre;
        private IRepositoryPedidoPgtoMercadoLivre _repositoryPedidoPgtoMercadoLivre;
        private IRepositoryPedidoEnvioMercadoLivre _repositoryPedidoEnvioMercadoLivre;

        public PrincarValidarHandler(
            IMercadoLivreApi mercadoLivreApi,
            IRepositoryPedidoMercadoLivre repositoryPedidoMercadoLivre,
            IRepositoryPedidoItemMercadoLivre repositoryPedidoItemMercadoLivre,
            IRepositoryPedidoPgtoMercadoLivre repositoryPedidoPgtoMercadoLivre,
            IRepositoryPedidoEnvioMercadoLivre repositoryPedidoEnvioMercadoLivre,
            IUnitOfWork unitOfWork)
        {
            _mercadoLivreApi = mercadoLivreApi;
            _repositoryPedidoMercadoLivre = repositoryPedidoMercadoLivre;
            _repositoryPedidoItemMercadoLivre = repositoryPedidoItemMercadoLivre;
            _repositoryPedidoPgtoMercadoLivre = repositoryPedidoPgtoMercadoLivre;
            _repositoryPedidoEnvioMercadoLivre = repositoryPedidoEnvioMercadoLivre;
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
                Guid empresaId = Guid.Parse("00000000-0000-0000-0000-000000000000");

                if (request.UsuarioId == "1367001737")
                    empresaId = Guid.Parse("23bacfdc-e577-4acb-b5f3-964547dec026");
                else if (request.UsuarioId == "2552128332")
                    empresaId = Guid.Parse("4feb3ea6-878d-4f59-b9ec-c89f221a7433");
                else
                {
                    AddNotification("PrincarValidar", "Usuário não reconhecido.");
                    return new CommandResponse(this);
                }

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

                // Buscar o envio do pedido usando o ID do envio
                var pedidoEnvio = _mercadoLivreApi.BuscarPedidoEnvio(pedido.shipping?.id.ToString(), token.access_token);

                if (pedidoEnvio == null)
                {
                    AddNotification("PrincarValidar", "Pedido envio não encontrado.");
                    return new CommandResponse(this);
                }

                // Buscar Nota Fiscal do Pedido
                var notaFiscal = _mercadoLivreApi.BuscarNotaFiscal(pedidoId, token.access_token);

                var cpf = "";
                var cnpj = "";
                var cpfcnpj = "";

                if (notaFiscal != null)
                {
                    cpf = notaFiscal.recipient?.identifications.cpf ?? string.Empty;
                    cnpj = notaFiscal.recipient?.identifications.cnpj ?? string.Empty;
                    cpfcnpj = "";

                    if (!string.IsNullOrEmpty(cpf))
                    {
                        cpfcnpj = cpf;
                    }
                    else if (!string.IsNullOrEmpty(cnpj))
                    {
                        cpfcnpj = cnpj;
                    }
                }

                int numeroNF = 0;
                var serieNF = string.Empty;

                if (notaFiscal != null)
                {
                    numeroNF = notaFiscal.invoice_number;
                    serieNF = notaFiscal.invoice_series;
                }

                //******************************************************//
                // Procedimentos para salvar ou atualizar o pedido
                //******************************************************//
                var nomeCliente = $"{pedido.buyer?.first_name ?? string.Empty} {pedido.buyer?.last_name ?? string.Empty}".Trim();
                var totalPedidoFormatado = pedido.total_amount.ToString("N2", new System.Globalization.CultureInfo("pt-BR"));
                var totalPagoFormatado = pedido.paid_amount.ToString("N2", new System.Globalization.CultureInfo("pt-BR"));
                var totalTaxasFormatado = pedido.taxes?.amount?.ToString("N2", new System.Globalization.CultureInfo("pt-BR")) ?? "0,00";
                var taxaEnvioFormatado = pedidoEnvio.shipping_option.list_cost?.ToString("N2", new System.Globalization.CultureInfo("pt-BR")) ?? "0,00";
                var receitaEnvioFormatado = pedidoEnvio.shipping_option.cost?.ToString("N2", new System.Globalization.CultureInfo("pt-BR")) ?? "0,00";

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
                        TipoEntrega = pedidoEnvio.logistic_type,
                        NumeroNF = numeroNF,
                        SerieNF = serieNF,
                        TaxaEnvio = string.IsNullOrEmpty(taxaEnvioFormatado) ? null : Convert.ToDecimal(taxaEnvioFormatado),
                        CnpjCpf = cpfcnpj,
                        ReceitaEnvio = string.IsNullOrEmpty(receitaEnvioFormatado) ? null : Convert.ToDecimal(receitaEnvioFormatado)
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
                        pedidoMercadoLivre.TipoEntrega = pedidoEnvio.logistic_type;
                        pedidoMercadoLivre.NumeroNF = numeroNF;
                        pedidoMercadoLivre.SerieNF = serieNF;
                        pedidoMercadoLivre.TaxaEnvio = string.IsNullOrEmpty(taxaEnvioFormatado) ? null : Convert.ToDecimal(taxaEnvioFormatado);
                        pedidoMercadoLivre.CnpjCpf = cpfcnpj;
                        pedidoMercadoLivre.ReceitaEnvio = string.IsNullOrEmpty(receitaEnvioFormatado) ? null : Convert.ToDecimal(receitaEnvioFormatado);

                        _repositoryPedidoMercadoLivre.Update(pedidoMercadoLivre);
                    }
                }

                //**************************************************************//
                // Procedimento para salvar ou atualizar o envio do pedido
                //**************************************************************//
                var custoBase = pedidoEnvio.base_cost.ToString("N2", new System.Globalization.CultureInfo("pt-BR")) ?? "0,00";
                var custoPedido = pedidoEnvio.order_cost.ToString("N2", new System.Globalization.CultureInfo("pt-BR")) ?? "0,00";

                var pedidoEnvioExiste = await _repositoryPedidoEnvioMercadoLivre.ExistsAsync(p => p.PedidoId == pedidoId, cancellationToken);

                if (!pedidoEnvioExiste)
                {
                    // Criar um novo envio do pedido na tabela PedidoEnvioMercadoLivre
                    var pedidoEnvioMercadoLivre = new PedidoEnvioMercadoLivre
                    {
                        Id = Guid.NewGuid(),
                        PedidoId = pedidoId,
                        CustoBase = Convert.ToDecimal(custoBase),
                        CustoPedido = Convert.ToDecimal(custoPedido),
                        DataEnvio = pedidoEnvio.status_history.date_shipped,
                        DataRetorno = pedidoEnvio.status_history.date_returned,
                        DataEntrega = pedidoEnvio.status_history.date_delivered,
                        DataVisita = pedidoEnvio.status_history.date_first_visit,
                        DataNaoEntrega = pedidoEnvio.status_history.date_not_delivered,
                        DataCancelamento = pedidoEnvio.status_history.date_cancelled,
                        DataManipulacao = pedidoEnvio.status_history.date_handling,
                        DataLiberacaoEntrega = pedidoEnvio.status_history.date_ready_to_ship
                    };

                    await _repositoryPedidoEnvioMercadoLivre.AddAsync(pedidoEnvioMercadoLivre, cancellationToken);
                }
                else
                {
                    // Atualizar o envio do pedido existente na tabela PedidoEnvioMercadoLivre
                    var pedidoEnvioMercadoLivre = _repositoryPedidoEnvioMercadoLivre.GetByAsync(false, p => p.PedidoId == pedidoId, cancellationToken);

                    if (pedidoEnvioMercadoLivre != null)
                    {
                        var envioMercadoLivre = pedidoEnvioMercadoLivre.Result;

                        envioMercadoLivre.CustoBase = Convert.ToDecimal(custoBase);
                        envioMercadoLivre.CustoPedido = Convert.ToDecimal(custoPedido);
                        envioMercadoLivre.DataEnvio = pedidoEnvio.status_history.date_shipped;
                        envioMercadoLivre.DataRetorno = pedidoEnvio.status_history.date_returned;
                        envioMercadoLivre.DataEntrega = pedidoEnvio.status_history.date_delivered;
                        envioMercadoLivre.DataVisita = pedidoEnvio.status_history.date_first_visit;
                        envioMercadoLivre.DataNaoEntrega = pedidoEnvio.status_history.date_not_delivered;
                        envioMercadoLivre.DataCancelamento = pedidoEnvio.status_history.date_cancelled;
                        envioMercadoLivre.DataManipulacao = pedidoEnvio.status_history.date_handling;
                        envioMercadoLivre.DataLiberacaoEntrega = pedidoEnvio.status_history.date_ready_to_ship;

                        _repositoryPedidoEnvioMercadoLivre.Update(envioMercadoLivre);
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
                        var taxaVendaFormatada = item.sale_fee?.ToString("N2", new System.Globalization.CultureInfo("pt-BR")) ?? "0,00";

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
