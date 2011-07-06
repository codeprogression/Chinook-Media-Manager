using System.Windows.Controls;

namespace ChinookMediaManager.Prism.AlbumsModule.Toolbar
{
    public partial class OpenAlbumToolBarButton : UserControl
    {
        public OpenAlbumToolBarButton(AlbumToolbarButtonViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}
