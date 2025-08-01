﻿using MercadoLivre.Core.Domain.Entities.Base;
using MercadoLivre.Core.Domain.Interfaces;

namespace MercadoLivre.Seguranca.Domain.Entities
{
    public class PedidoMercadoLivre : EntityBase<string>, IAggregateRoot
    {
        public string EmpresaId { get; set; }
        public DateTime? DataCriacao { get; set; }
        public DateTime? DataAlteracao { get; set; }
        public DateTime? DataFechamento { get; set; }
        public string PackId { get; set; }
        public decimal TotalPedido { get; set; }
        public decimal TotalPago { get; set; }
        public string ShippingId { get; set; }
        public string Status { get; set; }
        public string VendedorId { get; set; }
        public string ClienteId { get; set; }
        public string NickNameCliente { get; set; }
        public string NomeCliente { get; set; }
        public decimal? TotalTaxas { get; set; }
        public string TaxasId { get; set; }

        // EF
        public PedidoMercadoLivre() { }

        public PedidoMercadoLivre(string id, string empresaId, DateTime? dataCriacao, DateTime? dataAlteracao, DateTime? dataFechamento, string packId, decimal totalPedido, decimal totalPago, string shippingId, string status, string vendedorId, string clienteId, string nickNameCliente, string nomeCliente, decimal? totalTaxas = null, string taxasId = null)
        {
            Id = id;
            EmpresaId = empresaId;
            DataCriacao = dataCriacao;
            DataAlteracao = dataAlteracao;
            DataFechamento = dataFechamento;
            PackId = packId;
            TotalPedido = totalPedido;
            TotalPago = totalPago;
            ShippingId = shippingId;
            Status = status;
            VendedorId = vendedorId;
            ClienteId = clienteId;
            NickNameCliente = nickNameCliente;
            NomeCliente = nomeCliente;
            TotalTaxas = totalTaxas;
            TaxasId = taxasId;
        }

        public void Atualizar(DateTime? dataAlteracao, DateTime? dataFechamento, string packId, decimal totalPedido, decimal totalPago, string shippingId, string status, string vendedorId, string clienteId, string nickNameCliente, string nomeCliente, decimal? totalTaxas = null, string taxasId = null)
        {
            DataAlteracao = dataAlteracao;
            DataFechamento = dataFechamento;
            PackId = packId;
            TotalPedido = totalPedido;
            TotalPago = totalPago;
            ShippingId = shippingId;
            Status = status;
            VendedorId = vendedorId;
            ClienteId = clienteId;
            NickNameCliente = nickNameCliente;
            NomeCliente = nomeCliente;
            TotalTaxas = totalTaxas;
            TaxasId = taxasId;
        }
    }
}
