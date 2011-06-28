using ChinookMediaManager.Core.Persistence;

namespace ChinookMediaManager.Domain.Entities
{
	public class MediaType : Entity
	{
		public virtual string Name { get; set; }
	}
}