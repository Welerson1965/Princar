using Princar.Core.Domain.Entities.Base;
using Princar.Core.Domain.Interfaces;

namespace Princar.Seguranca.Domain.Entities.Produto
{
    public class ProdutoEntity : EntityBase<Guid>, IAggregateRoot
    {
        public string Descricao { get; private set; }
        public decimal Preco { get; private set; }
        public decimal Quantidade { get; private set; }
        public int CodigoProduto { get; private set; }

        //EF
        public ProdutoEntity() { }

        public ProdutoEntity(string descricao, decimal preco, decimal quantidade, int codigoProduto)
        {
            Descricao = descricao;
            Preco = preco;
            Quantidade = quantidade;
            CodigoProduto = codigoProduto;
        }
    }
}
