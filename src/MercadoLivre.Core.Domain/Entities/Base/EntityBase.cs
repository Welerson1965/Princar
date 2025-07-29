using prmToolkit.NotificationPattern;

namespace MercadoLivre.Core.Domain.Entities.Base
{
    public abstract class EntityBase<TId> : Notifiable
    {
        public TId Id { get; set; }
    }
}
