namespace Princar.Seguranca.Domain.Commands.Produto
{
    public class ProdutoResponse
    {
        public string Descricao { get; set; }
        public decimal Preco { get; set; }
        public decimal Quantidade { get; set; }
        public int CodigoProduto { get; set; }
        public string Marca { get; set; }
    }
}
