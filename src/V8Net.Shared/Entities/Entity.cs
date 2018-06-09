using FluentValidator;

namespace V8Net.Shared.Entities
{
    public abstract class Entity : Notifiable
    {
        public int Id { get; protected set; }

        public override string ToString() => $"[{ GetType().Name } - Id: { Id } ]";
    }
}
