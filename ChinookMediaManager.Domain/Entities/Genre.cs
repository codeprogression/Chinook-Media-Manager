using ChinookMediaManager.Core.Persistence;

namespace ChinookMediaManager.Domain.Entities
{
	public class Genre : Entity
	{
		public virtual string Name { get; set; }
	}
}