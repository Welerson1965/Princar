using MercadoLivre.Core.Domain.Entities.Base;
using MercadoLivre.Core.Domain.Interfaces;
using System.Reflection.Metadata.Ecma335;

namespace MercadoLivre.Seguranca.Domain.Entities
{
    public class PedidoEnvioMercadoLivre : EntityBase<Guid>, IAggregateRoot
    {
        public string PedidoId { get; set; }
        public decimal CustoBase { get; set; }
        public decimal? CustoPedido { get; set; }
        public DateTime? DataEnvio { get; set; }
        public DateTime? DataRetorno { get; set; }
        public DateTime? DataEntrega { get; set; }
        public DateTime? DataVisita { get; set; }
        public DateTime? DataNaoEntrega { get; set; }
        public DateTime? DataCancelamento { get; set; }
        public DateTime? DataManipulacao { get; set; }
        public DateTime? DataLiberacaoEntrega { get; set; }

        // Navegabilidade
        public virtual PedidoMercadoLivre Pedido { get; set; }

        // EF
        public PedidoEnvioMercadoLivre() { }

        public PedidoEnvioMercadoLivre(string pedidoId, decimal custoBase, decimal? custoPedido, DateTime? dataEnvio, DateTime? dataRetorno, DateTime? dataEntrega, DateTime? dataVisita, DateTime? dataNaoEntrega, DateTime? dataCancelamento, DateTime? dataManipulacao, DateTime? dataLiberacaoEntrega)
        {
            Id = Guid.NewGuid();
            PedidoId = pedidoId;
            CustoBase = custoBase;
            CustoPedido = custoPedido;
            DataEnvio = dataEnvio;
            DataRetorno = dataRetorno;
            DataEntrega = dataEntrega;
            DataVisita = dataVisita;
            DataNaoEntrega = dataNaoEntrega;
            DataCancelamento = dataCancelamento;
            DataManipulacao = dataManipulacao;
            DataLiberacaoEntrega = dataLiberacaoEntrega;
        }

        public void Atualizar(decimal custoBase, decimal? custoPedido, DateTime? dataEnvio, DateTime? dataRetorno, DateTime? dataEntrega, DateTime? dataVisita, DateTime? dataNaoEntrega, DateTime? dataCancelamento, DateTime? dataManipulacao, DateTime? dataLiberacaoEntrega)
        {
            CustoBase = custoBase;
            CustoPedido = custoPedido;
            DataEnvio = dataEnvio;
            DataRetorno = dataRetorno;
            DataEntrega = dataEntrega;
            DataVisita = dataVisita;
            DataNaoEntrega = dataNaoEntrega;
            DataCancelamento = dataCancelamento;
            DataManipulacao = dataManipulacao;
            DataLiberacaoEntrega = dataLiberacaoEntrega;
        }
    }
}
