using MediatR;
using Princar.Core.Domain.DTOs;

namespace Princar.Seguranca.Domain.Commands.Produto
{
    public class ProdutoRequest : IRequest<CommandResponse>
    {
        public string Descricao { get; set; }
    }
}
