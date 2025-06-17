using System.ComponentModel;

namespace Princar.Seguranca.Domain.Enumerators.Seguranca
{
    public enum FlagLicencaUso
    {
        [Description("Ativo")]
        Ativo = 1,
        [Description("Inativo")]
        Inativo = 2,
        [Description("Bloqueado")]
        Bloqueado = 3
    }
}
