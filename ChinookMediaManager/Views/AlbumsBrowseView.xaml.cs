using ChinookMediaManager.ViewModels;

using StructureMap;

namespace ChinookMediaManager.Views
{
	public partial class AlbumsBrowseView
	{
		public AlbumsBrowseView(IContainer container)
		{
		    InitializeComponent();
		    DataContext = container.GetInstance<AlbumsBrowseViewModel>();
		}
	}
}
