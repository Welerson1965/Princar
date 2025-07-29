using MediatR;
using MercadoLivre.Core.Domain.DTOs;

namespace MercadoLivre.Seguranca.Domain.Commands.Princar.Validar
{
    public class PrincarValidarRequest : IRequest<CommandResponse>
    {
        public string Recurso { get; set; }
    }
}
