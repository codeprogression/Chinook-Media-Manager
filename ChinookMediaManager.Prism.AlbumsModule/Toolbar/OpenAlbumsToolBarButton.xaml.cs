using System.Windows.Controls;

namespace ChinookMediaManager.Prism.AlbumsModule.Toolbar
{
    public partial class OpenAlbumsToolBarButton : UserControl
    {
        public OpenAlbumsToolBarButton(AlbumToolbarButtonViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}
