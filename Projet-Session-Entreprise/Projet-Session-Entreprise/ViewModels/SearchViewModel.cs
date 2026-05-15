using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Projet_Session_Entreprise.Models;
using Projet_Session_Entreprise.Repositories.Interfaces;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace Projet_Session_Entreprise.ViewModels
{
    public partial class SearchViewModel : ObservableObject
    {
        private readonly ITutorRepository _tutorRepo;

        [ObservableProperty] private string _searchText = "";
        [ObservableProperty] private string _selectedFilter = "Nom";

        public ObservableCollection<Tutor> Results { get; set; } = new ObservableCollection<Tutor>();

        public SearchViewModel(ITutorRepository tutorRepo)
        {
            _tutorRepo = tutorRepo;
            _ = InitialLoadAsync();
        }

        private async Task InitialLoadAsync()
        {
            var tuteurs = await _tutorRepo.GetAllAsync();
            Results.Clear();
            foreach (var t in tuteurs) Results.Add(t);
        }

        [RelayCommand]
        public async Task RechercherAsync()
        {
            if (string.IsNullOrWhiteSpace(SearchText))
            {
                await InitialLoadAsync();
                return;
            }

            var results = SelectedFilter == "Matière"
                ? await _tutorRepo.GetBySubjectAsync(SearchText)
                : await _tutorRepo.SearchTutorsAsync(SearchText);

            Results.Clear();
            foreach (var t in results) Results.Add(t);
        }
    }
}