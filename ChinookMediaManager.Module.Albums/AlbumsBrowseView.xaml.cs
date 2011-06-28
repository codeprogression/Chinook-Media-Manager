namespace ChinookMediaManager.Module.Albums
{
	public partial class AlbumsBrowseView
	{
        public AlbumsBrowseView(AlbumsBrowseViewModel viewModel)
		{
		    InitializeComponent();
		    DataContext = viewModel;
		}
	}
}
