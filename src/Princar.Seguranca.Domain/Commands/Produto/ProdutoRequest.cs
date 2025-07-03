using MediatR;
using Princar.Core.Domain.DTOs;
using Princar.Seguranca.Domain.Enumerators;

namespace Princar.Seguranca.Domain.Commands.Produto
{
    public class ProdutoRequest : IRequest<CommandResponse>
    {
        public string Descricao { get; set; }
        public string Iniciando { get; set; }
    }
}
