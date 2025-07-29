namespace MercadoLivre.Seguranca.Domain.Commands.Princar.Validar
{
    public class PrincarValidarResponse(string mensagem)
    {
        public string Mensagem { get; } = mensagem;
    }
}
