using ChinookMediaManager.Core.Persistence;

namespace ChinookMediaManager.Domain
{
	public class MediaType : Entity
	{
		public virtual string Name { get; set; }
	}
}