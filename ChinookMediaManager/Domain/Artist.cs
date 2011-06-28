using ChinookMediaManager.Core.Persistence;

namespace ChinookMediaManager.Domain
{
	public class Artist : Entity
	{
		public virtual string Name { get; set; }
	}
}