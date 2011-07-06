using System;
using System.ComponentModel;
using ChinookMediaManager.Domain.Entities;
using ChinookMediaManager.Prism.AlbumsModule.Browse;
using Machine.Specifications;
using Microsoft.Practices.Prism.Events;

namespace ChinookMediaManager.Prism.Specs.AlbumBrowseViewModelSpecs
{
    [Subject(typeof(AlbumsBrowseViewModel))]
    public class when_updating_album
    {
        Establish context = () =>
            {
                SpecificationContext.Session.Save(new Album());
                _viewModel = new AlbumsBrowseViewModel(SpecificationContext.Session, new EventAggregator());
                _albumViewModel = _viewModel.Model[0];
                _lastPlayed = ((DateTime?)_albumViewModel.Model.LastPlayed).GetValueOrDefault();
                _albumViewModel.PropertyChanged += (s, e) => { _propertyChanged = e; };
            };

        Because of = () => _albumViewModel.UpdatePlayed(SpecificationContext.Session);

        It should_raise_property_changed_on__LastPlayed__ = () => 
            _propertyChanged.PropertyName.ShouldEqual("LastPlayed");
        
        It __LastPlayed__should_have_updated_to_new_time = () => 
            ((DateTime?)_albumViewModel.Model.LastPlayed).GetValueOrDefault().ShouldBeGreaterThan(_lastPlayed);
        
        static AlbumsBrowseViewModel _viewModel;
        static DateTime? _lastPlayed;
        static AlbumViewModel _albumViewModel;
        static PropertyChangedEventArgs _propertyChanged;
    }
}