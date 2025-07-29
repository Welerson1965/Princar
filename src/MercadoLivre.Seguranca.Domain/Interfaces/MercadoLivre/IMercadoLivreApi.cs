using MercadoLivre.DTOs;

namespace MercadoLivre.Seguranca.Domain.Interfaces.MercadoLivre
{
    public interface IMercadoLivreApi
    {
        TokenRetornoDTO BuscarToken(Guid empresaId);

        PedidoIndividualRetornoDTO BuscarPedidoIndividual(string pedidoId, string token);
    }
}
