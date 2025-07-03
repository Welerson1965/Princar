using MediatR;
using Princar.Core.Domain.DTOs;
using Princar.Seguranca.Domain.Entities.Produto;
using Princar.Seguranca.Domain.Interfaces.Repositories;
using prmToolkit.NotificationPattern;

namespace Princar.Seguranca.Domain.Commands.Produto
{
    public class ProdutoHandler : Notifiable, IRequestHandler<ProdutoRequest, CommandResponse>
    {
        private readonly IRepositoryProduto _repositoryProduto;

        public ProdutoHandler(IRepositoryProduto repositoryProduto)
        {
            _repositoryProduto = repositoryProduto;
        }

        public Task<CommandResponse> Handle(ProdutoRequest request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                AddNotification("Produto", "Dados do produto não enviados");
                return Task.FromResult(new CommandResponse(this));
            }

            if (string.IsNullOrWhiteSpace(request.Descricao))
            {
                AddNotification("Produto", "Descrição do produto não informada");
                return Task.FromResult(new CommandResponse(this));
            }

            // Divide a descrição em palavras, ignorando espaços extras
            var termos = request.Descricao
                .Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

            IEnumerable<ProdutoEntity> produtosFiltrados;

            if (request.Iniciando == "S" && termos.Length > 0)
            {
                var primeiroTermo = termos[0];
                var outrosTermos = termos.Skip(1).ToArray();

                produtosFiltrados = _repositoryProduto.ListarPorSemRastreamento(
                    p => !string.IsNullOrEmpty(p.Descricao) &&
                         p.Descricao.StartsWith(primeiroTermo, StringComparison.OrdinalIgnoreCase) &&
                         outrosTermos.All(t => p.Descricao.Contains(t, StringComparison.OrdinalIgnoreCase))
                );
            }
            else
            {
                produtosFiltrados = _repositoryProduto.ListarPorSemRastreamento(
                    p => !string.IsNullOrEmpty(p.Descricao) &&
                         termos.All(t => p.Descricao.Contains(t, StringComparison.OrdinalIgnoreCase))
                );
            }

            // Garante que cada CodigoProduto seja único no resultado
            var produtos = produtosFiltrados
                .DistinctBy(p => p.CodigoProduto)
                .ToList();

            return Task.FromResult(new CommandResponse(produtos, this));
        }
    }
}
