using ChinookMediaManager.Core.Persistence;

namespace ChinookMediaManager.Domain
{
	public class Genre : Entity
	{
		public virtual string Name { get; set; }
	}
}