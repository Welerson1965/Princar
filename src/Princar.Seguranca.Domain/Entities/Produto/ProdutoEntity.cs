﻿using Princar.Core.Domain.Entities.Base;
using Princar.Core.Domain.Interfaces;

namespace Princar.Seguranca.Domain.Entities.Produto
{
    public class ProdutoEntity : EntityBase<int>, IAggregateRoot
    {
        public string Descricao { get; private set; }
        public decimal Preco { get; private set; }
        public decimal Quantidade { get; private set; }
        public int CodigoProduto { get; private set; }
        public string MarcaProduto { get; set; }
        public string Unidade { get; set; }

        //EF
        public ProdutoEntity() { }

        public ProdutoEntity(string descricao, decimal preco, decimal quantidade, int codigoProduto, string marcaProduto, string unidade)
        {
            Descricao = descricao;
            Preco = preco;
            Quantidade = quantidade;
            CodigoProduto = codigoProduto;
            MarcaProduto = marcaProduto;
            Unidade = unidade;
        }
    }
}
