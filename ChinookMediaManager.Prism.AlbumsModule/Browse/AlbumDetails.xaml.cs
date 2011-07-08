using System.Windows;

namespace ChinookMediaManager.Prism.AlbumsModule.Browse
{
    public partial class AlbumDetails : Window
    {
        public AlbumDetails(AlbumViewModel model)
        {
            InitializeComponent();
            DataContext = model;
        }
    }
}
