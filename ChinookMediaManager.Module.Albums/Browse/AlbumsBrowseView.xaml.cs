namespace ChinookMediaManager.Module.Albums.Browse
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
