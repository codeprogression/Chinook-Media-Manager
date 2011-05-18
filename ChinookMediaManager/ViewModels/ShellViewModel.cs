using System.ComponentModel;
using System.Windows.Input;
using ChinookMediaManager.Views;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Regions;

namespace ChinookMediaManager.ViewModels
{
    public class ShellViewModel : INotifyPropertyChanged
    {
        private readonly IRegionManager _regionManager;

        public ShellViewModel(IRegionManager regionManager)
        {
            _regionManager = regionManager;
            ShowAlbumsCommand = new DelegateCommand<string>(ShowAlbumsExecute, ShowAlbumsCanExecute);
        }

        private void ShowAlbumsExecute(string obj)
        {
            _regionManager.RegisterViewWithRegion("ContentRegion", typeof (AlbumsBrowseView));
        }

        private bool ShowAlbumsCanExecute(string arg)
        {
            return true;
        }

        public ICommand ShowAlbumsCommand { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
