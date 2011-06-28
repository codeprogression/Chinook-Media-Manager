using System.Collections.ObjectModel;
using Microsoft.Practices.Prism.Commands;

namespace ChinookMediaManager.Core.DynamicViewModel
{
    public abstract class CollectionViewModelProxy<VIEWMODEL, MODEL> : ModelProxy<VIEWMODEL, MODEL>, ICollectionViewModel<MODEL>
        where MODEL : class, new()
        where VIEWMODEL : ViewModelProxy<VIEWMODEL, MODEL>
    {
        private bool _isVisible;
        public bool IsVisible
        {
            get { return _isVisible; }
            set
            {
                _isVisible = value;
                NotifyPropertyChanged(p => IsVisible);
            }
        }

        private bool _isBusy;
        public bool IsBusy
        {
            get { return _isBusy; }
            protected set
            {
                _isBusy = value;
                NotifyPropertyChanged(p => IsBusy);
            }
        }

        private VIEWMODEL _selectedItem;
        public VIEWMODEL SelectedItem
        {
            get
            {
                return _selectedItem;
            }
            set
            {
                _selectedItem = value;
                NotifyPropertyChanged(p => SelectedItem);
            }
        }

        public ObservableCollection<VIEWMODEL> Model { get; protected set; }
        public DelegateCommand<VIEWMODEL> NewCommand { get; protected set; }
        public DelegateCommand<VIEWMODEL> EditCommand { get; protected set; }
        public DelegateCommand<VIEWMODEL> DeleteCommand { get; protected set; }
        public DelegateCommand<object> RefreshCommand { get; protected set; }

        protected CollectionViewModelProxy()
        {
            Model = new ObservableCollection<VIEWMODEL>();
            RegisterCommands();
        }


        private void RegisterCommands()
        {
          /*  EditCommand =
                new DelegateCommand<VIEWMODEL>(
                    item => Container.GetInstance<IEditorViewFactory<MODEL>>().ShowView(SelectedItem.GetId()),
                    item => true/*SelectedItem != null#1#);
            NewCommand =
                new DelegateCommand<VIEWMODEL>(
                    item => Container.GetInstance<IEditorViewFactory<MODEL>>().ShowView(0),
                    item => true);
            DeleteCommand =
                new DelegateCommand<VIEWMODEL>(
                    item => Container.GetInstance<IDeleteViewFactory<MODEL>>().ShowDeleteView(SelectedItem.GetId()),
                    item => true);*/
//            RefreshCommand = new DelegateCommand<object>(item => Load(), item => true);
        }

        protected internal virtual void Load() { }

        protected override void RaiseCanExecuteEvents()
        {
//            EditCommand.RaiseCanExecuteChanged();
//            NewCommand.RaiseCanExecuteChanged();
//            DeleteCommand.RaiseCanExecuteChanged();
//            RefreshCommand.RaiseCanExecuteChanged();
        }

    }
    public interface ICollectionViewModel<T> //: IViewModel
    {
    }

}