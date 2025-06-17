using MediatR;
using Princar.Core.Domain.DTOs;

namespace Princar.Seguranca.Domain.Commands.Seguranca
{
    public class AutenticarRequest : IRequest<CommandResponse>
    {
        public string KeyId { get; set; }
    }
}
