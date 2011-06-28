namespace ChinookMediaManager.Core.Persistence
{
    public abstract class Entity : IPersistable
    {
        public virtual int Id { get; set; }
    }
}