using ChinookMediaManager.Domain.Entities;
using ChinookMediaManager.Prism.AlbumsModule.Browse;
using Machine.Specifications;

namespace ChinookMediaManager.Prism.Specs.AlbumBrowseViewModelSpecs
{
    [Subject(typeof(AlbumsBrowseViewModel))]
    public class when_requesting_album_list
    {
        Establish context = () =>
            {
                for (var i = 0; i < 10; i++)
                    SpecificationContext.Session.Save(new Album());
            };

        Because of = () => _viewModel = new AlbumsBrowseViewModel(SpecificationContext.Session);

        It should_retrieve_all_albums = () => _viewModel.Model.Count.ShouldEqual(10);
        
        static AlbumsBrowseViewModel _viewModel;
    }
}