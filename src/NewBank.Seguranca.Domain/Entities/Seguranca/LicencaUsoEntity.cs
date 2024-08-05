using NewBank.Core.Domain.Entities.Base;
using NewBank.Core.Domain.Interfaces;
using NewBank.Seguranca.Domain.Enumerators.Seguranca;

namespace NewBank.Seguranca.Domain.Entities.Seguranca
{
    public class LicencaUsoEntity : EntityBase<Guid>, IAggregateRoot
    {
        public string NomeEmpresa { get; private set; }
        public string NomeContato { get; private set; }
        public string Telefone { get; private set; }
        public string Email { get; private set; }
        public FlagLicencaUso FlagLicencaUso { get; private set; }

        //EF
        public LicencaUsoEntity() { }

        public LicencaUsoEntity(string nomeEmpresa, string nomeContato, string telefone, string email, FlagLicencaUso flagLicencaUso)
        {
            NomeEmpresa = nomeEmpresa;
            NomeContato = nomeContato;
            Telefone = telefone;
            Email = email;
            FlagLicencaUso = flagLicencaUso;
        }
    }
}
