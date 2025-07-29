using MercadoLivre.Core.Domain.Entities.Base;
using MercadoLivre.Core.Domain.Interfaces;

namespace MercadoLivre.Seguranca.Domain.Entities
{
    public class PedidoPgtoMercadoLivre : EntityBase<Guid>, IAggregateRoot
    {
        public string PedidoId { get; set; }
        public string IdML { get; set; }
        public string PagamentoId { get; set; }
        public string ColetorId { get; set; }
        public string CartaoId { get; set; }
        public string SiteId { get; set; }
        public string TipoPagamento { get; set; }
        public string Status { get; set; }
        public string StatusDetalhe { get; set; }
        public decimal ValorPagto { get; set; }
        public decimal? ValorDevolvida { get; set; }
        public decimal? ValorTaxas { get; set; }
        public decimal? ValorCupom { get; set; }
        public decimal? ValorAcrescimo { get; set; }
        public decimal? TotalPagamento { get; set; }
        public DateTime? DataAprovacao { get; set; }
        public DateTime? DataCriacao { get; set; }
        public DateTime? DataModificacao { get; set; }
        public decimal? TaxaMarketPlace { get; set; }

        //EF
        public PedidoPgtoMercadoLivre() { }

        public PedidoPgtoMercadoLivre(string pedidoId, string idML, string pagamentoId, string coletorId, string cartaoId, string siteId, string tipoPagamento, string status, string statusDetalhe, decimal valorPagto, decimal? valorDevolvida, decimal? valorTaxas, decimal? valorCupom, decimal? valorAcrescimo, decimal? totalPagamento, DateTime? dataAprovacao, DateTime? dataCriacao, DateTime? dataModificacao, decimal? taxaMarketPlace)
        {
            Id = Guid.NewGuid();
            PedidoId = pedidoId;
            IdML = idML;
            PagamentoId = pagamentoId;
            ColetorId = coletorId;
            CartaoId = cartaoId;
            SiteId = siteId;
            TipoPagamento = tipoPagamento;
            Status = status;
            StatusDetalhe = statusDetalhe;
            ValorPagto = valorPagto;
            ValorDevolvida = valorDevolvida;
            ValorTaxas = valorTaxas;
            ValorCupom = valorCupom;
            ValorAcrescimo = valorAcrescimo;
            TotalPagamento = totalPagamento;
            DataAprovacao = dataAprovacao;
            DataCriacao = dataCriacao;
            DataModificacao = dataModificacao;
            TaxaMarketPlace = taxaMarketPlace;
        }

        public void Atualizar(string status, string statusDetalhe, decimal valorPagto, decimal? valorDevolvida, decimal? valorTaxas, decimal? valorCupom, decimal? valorAcrescimo, decimal? totalPagamento, DateTime? dataAprovacao, DateTime? dataModificacao, decimal? taxaMarketPlace)
        {
            Status = status;
            StatusDetalhe = statusDetalhe;
            ValorPagto = valorPagto;
            ValorDevolvida = valorDevolvida;
            ValorTaxas = valorTaxas;
            ValorCupom = valorCupom;
            ValorAcrescimo = valorAcrescimo;
            TotalPagamento = totalPagamento;
            DataAprovacao = dataAprovacao;
            DataModificacao = dataModificacao;
            TaxaMarketPlace = taxaMarketPlace;
        }
    }
}
