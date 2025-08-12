using MercadoLivre.DTOs;
using MercadoLivre.Seguranca.Domain.DTOs;
using MercadoLivre.Seguranca.Domain.Interfaces.MercadoLivre;
using MercadoLivre.Seguranca.Domain.Interfaces.Repositories;
using Newtonsoft.Json;
using prmToolkit.NotificationPattern;
using RestSharp;

namespace MercadoLivre.Infra.Data.External.MercadoLivre
{
    public class MercadoLivreApi : Notifiable, IMercadoLivreApi
    {
        private readonly IRepositoryMercadoLivre _repositoryMercadoLivre;

        public MercadoLivreApi(IRepositoryMercadoLivre repositoryMercadoLivre)
        {
            _repositoryMercadoLivre = repositoryMercadoLivre;
        }

        private string _IdClienteML;
        private string _ChaveSecretaML;
        private string _URIML;
        private string _CodeML;
        private string _TokenML;
        private string _RefreshTokenML;
        private string _VendedorML;

        //*********************************************************//
        // Chamada para buscar o Token do Mercado Livre
        //*********************************************************//
        public TokenRetornoDTO BuscarToken(Guid empresaId)
        {
            BuscarConfiguracoes(empresaId);

            var tokenRetornoDTO = new TokenRetornoDTO();

            var client = new RestClient($"https://api.mercadolibre.com/oauth/token");

            var request = new RestRequest(Method.POST)
                .AddHeader("Content-Type", "application/x-www-form-urlencoded")
                .AddHeader("Accept", "application/json")
                .AddParameter("grant_type", "refresh_token")
                .AddParameter("client_id", _IdClienteML)
                .AddParameter("client_secret", _ChaveSecretaML)
                .AddParameter("refresh_token", _RefreshTokenML);

            var response = client.Execute(request);

            if (response.IsSuccessful)
            {
                tokenRetornoDTO = JsonConvert.DeserializeObject<TokenRetornoDTO>(response.Content);

                return tokenRetornoDTO;
            }

            return null;
        }

        //*********************************************************//
        // Buscar o Pedido Indivualmente pelo ID
        //*********************************************************//
        public PedidoIndividualRetornoDTO BuscarPedidoIndividual(string pedidoId, string token)
        {
            var pedidoIndividualRetornoDTO = new PedidoIndividualRetornoDTO();

            var url = $"https://api.mercadolibre.com/orders/{pedidoId}";

            var client = new RestClient(url);

            var request = new RestRequest(Method.GET)
                .AddHeader("Authorization", $"Bearer {token}")
                .AddHeader("Accept", "application/json");

            var response = client.Execute(request);

            if (response.IsSuccessful)
            {
                pedidoIndividualRetornoDTO = JsonConvert.DeserializeObject<PedidoIndividualRetornoDTO>(response.Content);
                return pedidoIndividualRetornoDTO;
            }

            return null;
        }

        //*********************************************************//
        // Buscar o Envio do Pedido pelo ID
        //*********************************************************//
        public PedidoEnvioRetornoDTO BuscarPedidoEnvio(string envioId, string token)
        {
            var pedidoEnvioRetornoDTO = new PedidoEnvioRetornoDTO();

            var url = $"https://api.mercadolibre.com/shipments/{envioId}";

            var client = new RestClient(url);

            var request = new RestRequest(Method.GET)
                .AddHeader("Authorization", $"Bearer {token}")
                .AddHeader("Accept", "application/json");

            var response = client.Execute(request);

            if (response.IsSuccessful)
            {
                pedidoEnvioRetornoDTO = JsonConvert.DeserializeObject<PedidoEnvioRetornoDTO>(response.Content);
                return pedidoEnvioRetornoDTO;
            }

            return null;
        }

        //*********************************************************//
        // Buscar Nota Fiscal do Pedido pelo ID
        //*********************************************************//
        public NotaFiscalRetornoDTO BuscarNotaFiscal(string pedidoId, string token)
        {
            var notaFiscalRetornoDTO = new NotaFiscalRetornoDTO();

            var url = $"https://api.mercadolibre.com/users/134608322/invoices/orders/{pedidoId}";

            var client = new RestClient(url);

            var request = new RestRequest(Method.GET)
                .AddHeader("Authorization", $"Bearer {token}")
                .AddHeader("Accept", "application/json");

            var response = client.Execute(request);

            if (response.IsSuccessful)
            {
                notaFiscalRetornoDTO = JsonConvert.DeserializeObject<NotaFiscalRetornoDTO>(response.Content);
                return notaFiscalRetornoDTO;
            }

            return null;
        }

        //********************************************************//
        // Buscar parâmetros de configuração do Mercado Livre
        //********************************************************//
        public void BuscarConfiguracoes(Guid empresaId)
        {
            var configuracao = _repositoryMercadoLivre.ObterPorId(empresaId);

            if (configuracao == null)
            {
                AddNotification("Configuração", "Configuração do Mercado Livre não encontrada.");
                return;
            }

            _IdClienteML = configuracao.IdClienteML;
            _ChaveSecretaML = configuracao.ChaveSecretaML;
            _URIML = configuracao.URIML;
            _CodeML = configuracao.CodeML;
            _TokenML = configuracao.TokenML;
            _RefreshTokenML = configuracao.RefreshTokenML;
            _VendedorML = configuracao.VendedorML;
        }
    }
}
