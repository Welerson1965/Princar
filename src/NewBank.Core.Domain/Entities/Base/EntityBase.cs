using prmToolkit.NotificationPattern;

namespace NewBank.Core.Domain.Entities.Base
{
    public abstract class EntityBase<TId> : Notifiable
    {
        public TId Id { get; private set; }
    }
}
