using MercadoLivre.Core.Domain.Entities.Base;
using MercadoLivre.Core.Domain.Interfaces;

namespace MercadoLivre.Seguranca.Domain.Entities
{
    public class MercadoLivreEntities : EntityBase<Guid>, IAggregateRoot
    {
        public string Cnpj { get; set; }
        public string NomeEmpresa { get; set; }
        public string IdClienteML { get; set; }
        public string ChaveSecretaML { get; set; }
        public string URIML { get; set; }
        public string CodeML { get; set; }
        public string TokenML { get; set; }
        public string RefreshTokenML { get; set; }
        public string VendedorML { get; set; }

        //EF
        public MercadoLivreEntities() { }

        public MercadoLivreEntities(string cnpj, string nomeEmpresa, string idClienteML, string chaveSecretaML, string uriML, string codeML, string tokenML, string refreshTokenML, string vendedorML)
        {
            Id = Guid.NewGuid();
            Cnpj = cnpj;
            NomeEmpresa = nomeEmpresa;
            IdClienteML = idClienteML;
            ChaveSecretaML = chaveSecretaML;
            URIML = uriML;
            CodeML = codeML;
            TokenML = tokenML;
            RefreshTokenML = refreshTokenML;
            VendedorML = vendedorML;
        }

        public void Atualizar(string tokenML, string refreshTokenML)
        {
            TokenML = tokenML;
            RefreshTokenML = refreshTokenML;
        }
    }
}
