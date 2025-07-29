namespace MercadoLivre.Seguranca.Domain.Commands.Princar
{
    public class NotificacoesPrincarResponse(string mensagem)
    {
        public string Mensagem { get; } = mensagem;
    }
}