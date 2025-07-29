namespace MercadoLivre.Seguranca.Domain.Commands.Princar.Notificacoes
{
    public class NotificacoesPrincarResponse(string mensagem)
    {
        public string Mensagem { get; } = mensagem;
    }
}