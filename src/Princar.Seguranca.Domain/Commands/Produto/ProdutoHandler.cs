using MediatR;
using Princar.Core.Domain.DTOs;
using Princar.Seguranca.Domain.Commands.Seguranca;
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

            // Busca produtos cuja descrição contenha o texto informado (case-insensitive)
            var produtos = _repositoryProduto.ListarPorSemRastreamento(
                p => !string.IsNullOrEmpty(p.Descricao) &&
                     p.Descricao.Contains(request.Descricao, StringComparison.OrdinalIgnoreCase)
            );

            return Task.FromResult(new CommandResponse(produtos, this));
        }
    }
}
