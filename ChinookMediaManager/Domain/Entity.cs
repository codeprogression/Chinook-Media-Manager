using System.ComponentModel;
using ChinookMediaManager.Infrastructure.Persistence;

namespace ChinookMediaManager.Domain
{
    public abstract class Entity : IPersistable
    {
        public virtual int Id { get; set; }
    }
}