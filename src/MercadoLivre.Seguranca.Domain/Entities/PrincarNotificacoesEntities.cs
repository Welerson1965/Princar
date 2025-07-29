using MercadoLivre.Core.Domain.Entities.Base;
using MercadoLivre.Core.Domain.Interfaces;

namespace MercadoLivre.Seguranca.Domain.Entities
{
    public class PrincarNotificacoesEntities : EntityBase<string>, IAggregateRoot
    {
        public string Recurso { get; set; }
        public string UsuarioId { get; set; }
        public string Topico { get; set; }
        public string AplicacaoId { get; set; }
        public int Tentativas { get; set; }
        public DateTime DataEnvio { get; set; }
        public DateTime DataRecebimento { get; set; }

        // EF
        public PrincarNotificacoesEntities() { }

        public PrincarNotificacoesEntities(string recurso, string usuarioId, string topico, string aplicacaoId, int tentativas, DateTime dataEnvio, DateTime dataRecebimento)
        {
            Id = Guid.NewGuid().ToString(); // Gera um novo ID único
            Recurso = recurso;
            UsuarioId = usuarioId;
            Topico = topico;
            AplicacaoId = aplicacaoId;
            Tentativas = tentativas;
            DataEnvio = dataEnvio;
            DataRecebimento = dataRecebimento;
        }
    }
}
