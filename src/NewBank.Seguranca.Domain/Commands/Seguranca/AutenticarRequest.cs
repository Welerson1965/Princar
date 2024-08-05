using MediatR;
using NewBank.Core.Domain.DTOs;

namespace NewBank.Seguranca.Domain.Commands.Seguranca
{
    public class AutenticarRequest : IRequest<CommandResponse>
    {
        public string KeyId { get; set; }
    }
}
