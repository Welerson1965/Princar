using MercadoLivre.Core.Domain.Entities.Base;
using MercadoLivre.Core.Domain.Interfaces;
using System.Security.Principal;

namespace MercadoLivre.Seguranca.Domain.Entities
{
    public class PedidoItemMercadoLivreEntities : EntityBase<Guid>, IAggregateRoot 
    {
        public string PedidoId { get; set; }
        public string ItemId { get; set; }
        public string Descricao { get; set; }
        public string CategoriaId { get; set; }
        public string SKU { get; set; }
        public decimal Quantidade { get; set; }
        public decimal ValorUnitario { get; set; }
        public decimal ValorFullUnitario { get; set; }
        public string Moeda { get; set; }
        public decimal? TaxaVenda { get; set; }

        // Navegabilidade
        public virtual PedidoMercadoLivreEntities Pedido { get; set; }

        // EF
        public PedidoItemMercadoLivreEntities() { }

        public PedidoItemMercadoLivreEntities(string pedidoId, string itemId, string descricao, string categoriaId, string sku, decimal quantidade, decimal valorUnitario, decimal valorFullUnitario, string moeda, decimal? taxaVenda = null)
        {
            Id = Guid.NewGuid();
            PedidoId = pedidoId;
            ItemId = itemId;
            Descricao = descricao;
            CategoriaId = categoriaId;
            SKU = sku;
            Quantidade = quantidade;
            ValorUnitario = valorUnitario;
            ValorFullUnitario = valorFullUnitario;
            Moeda = moeda;
            TaxaVenda = taxaVenda;
        }

        public void Atualizar(decimal quantidade, decimal valorUnitario, decimal valorFullUnitario, string moeda, decimal? taxaVenda = null)
        {
            Quantidade = quantidade;
            ValorUnitario = valorUnitario;
            ValorFullUnitario = valorFullUnitario;
            Moeda = moeda;
            TaxaVenda = taxaVenda;
        }
    }
}
