using System.Collections.ObjectModel;
using ModelFuu;

namespace SampleApp
{
    public class MainWindowViewModel
    {
        public static ModelPropertyCollection<MainWindowViewModel, SearchInfo> SearchInfoProperties =
            ModelProperty<MainWindowViewModel>
            .CreateFrom<SearchInfo>()
            .PropertyChangedAny(a => a.OwnerInstance.searchController.DoSearch())
            .Build();

        public static ModelProperty SearchStatusProperty =
            ModelProperty<MainWindowViewModel>
            .Create<string>("SearchStatus")
            .Build();

        private SearchController searchController;

        public MainWindowViewModel()
        {
            searchController = new SearchController(this);
            PeopleResults = new ObservableCollection<PersonViewModel>();
        }

        public ObservableCollection<PersonViewModel> PeopleResults { get; private set; }
    }
}
