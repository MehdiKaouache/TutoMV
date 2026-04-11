using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Projet_Session_Entreprise.Services;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace Projet_Session_Entreprise.ViewModels
{
    public partial class SearchViewModel : ObservableObject
    {
        private readonly SearchService _searchService;

        [ObservableProperty] private string _searchText = "";
        [ObservableProperty] private string _selectedFilter = "Nom";

        public ObservableCollection<Tutor> Results { get; set; } = new ObservableCollection<Tutor>();

        public SearchViewModel()
        {
            _searchService = new SearchService(new AppDbContext());
        }

        [RelayCommand]
        private async Task RechercherAsync()
        {
            if (string.IsNullOrWhiteSpace(SearchText))
                return;

            var tuteurs = SelectedFilter == "Matière"
                ? await _searchService.SearchBySubjectAsync(SearchText)
                : await _searchService.SearchByNameAsync(SearchText);

            Results.Clear();
            foreach (var t in tuteurs)
                Results.Add(t);
        }
    }
}
