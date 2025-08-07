using MercadoLivre.DTOs;
using MercadoLivre.Seguranca.Domain.DTOs;

namespace MercadoLivre.Seguranca.Domain.Interfaces.MercadoLivre
{
    public interface IMercadoLivreApi
    {
        TokenRetornoDTO BuscarToken(Guid empresaId);
        PedidoIndividualRetornoDTO BuscarPedidoIndividual(string pedidoId, string token);
        PedidoEnvioRetornoDTO BuscarPedidoEnvio(string envioId, string token);
    }
}
