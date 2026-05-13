using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Projet_Session_Entreprise.Models;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace Projet_Session_Entreprise.ViewModels
{
    public partial class SearchViewModel : ObservableObject
    {
        [ObservableProperty] private string _searchText = "";
        [ObservableProperty] private string _selectedFilter = "Nom";

        public ObservableCollection<Tutor> Results { get; set; } = new ObservableCollection<Tutor>();

        public SearchViewModel()
        {
            _ = InitialLoadAsync();
        }

        private async Task InitialLoadAsync()
        {
            var tuteurs = await App.TutorRepo.GetAllAsync();
            Results.Clear();
            foreach (var t in tuteurs) Results.Add(t);
        }

        [RelayCommand]
        private async Task RechercherAsync()
        {
            if (string.IsNullOrWhiteSpace(SearchText))
            {
                await InitialLoadAsync();
                return;
            }

            var results = SelectedFilter == "Matière"
                ? await App.TutorRepo.GetBySubjectAsync(SearchText)
                : await App.TutorRepo.SearchTutorsAsync(SearchText);

            Results.Clear();
            foreach (var t in results) Results.Add(t);
        }
    }
}