using System.Windows.Controls;

namespace ChinookMediaManager.Prism.AlbumsModule.Toolbar
{
    public partial class AlbumToolbarButtonView : UserControl
    {
        public AlbumToolbarButtonView(AlbumToolbarButtonViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}
