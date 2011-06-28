using ChinookMediaManager.Core.Persistence;

namespace ChinookMediaManager.Domain
{
    public abstract class Entity : IPersistable
    {
        public virtual int Id { get; set; }
    }
}