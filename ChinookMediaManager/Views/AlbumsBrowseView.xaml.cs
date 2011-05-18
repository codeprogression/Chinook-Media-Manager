using ChinookMediaManager.ViewModels;
using Microsoft.Practices.Unity;

namespace ChinookMediaManager.Views
{
	public partial class AlbumsBrowseView
	{
		public AlbumsBrowseView(IUnityContainer container)
		{
		    InitializeComponent();
		    DataContext = container.Resolve<AlbumsBrowseViewModel>();
		}
	}
}
