namespace ChinookMediaManager.Prism.AlbumsModule.Browse
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
