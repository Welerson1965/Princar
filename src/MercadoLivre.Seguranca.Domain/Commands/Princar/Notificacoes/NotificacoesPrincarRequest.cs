using MediatR;
using MercadoLivre.Core.Domain.DTOs;

namespace MercadoLivre.Seguranca.Domain.Commands.Princar.Notificacoes
{
    public class NotificacoesPrincarRequest : IRequest<CommandResponse>
    {
        public string resource { get; set; }
        public string user_id { get; set; }
        public string topic { get; set; }
        public string application_id { get; set; }
        public int attempts { get; set; }
        public DateTime sent { get; set; }
        public DateTime received { get; set; }
    }
}
