using ChinookMediaManager.Core.Persistence;

namespace ChinookMediaManager.Domain.Entities
{
	public class Artist : Entity
	{
		public virtual string Name { get; set; }
	}
}