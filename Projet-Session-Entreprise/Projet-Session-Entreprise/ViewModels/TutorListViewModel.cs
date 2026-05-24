using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Projet_Session_Entreprise.Core.Models;
using Projet_Session_Entreprise.Core.Interfaces;
using Projet_Session_Entreprise.Infrastructure.Services;
using Projet_Session_Entreprise.UI.Views;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace Projet_Session_Entreprise.UI.ViewModels
{
    public partial class TutorListViewModel : ObservableObject
    {
        private readonly ITutorRepository _tutorRepo;
        private Student? _currentStudent;

        [ObservableProperty] private string _searchText = "";
        [ObservableProperty] private string _selectedFilter = "Nom";

        public ObservableCollection<string> FilterChoices { get; } = new() { "Nom", "Matière" };
        public ObservableCollection<Tutor> Results { get; set; } = new();

        public TutorListViewModel() : this(null, App.TutorRepo) { }

        public TutorListViewModel(Student? student) : this(student, App.TutorRepo) { }

        public TutorListViewModel(Student? student, ITutorRepository tutorRepo)
        {
            _currentStudent = student;
            _tutorRepo = tutorRepo;
            _ = InitialLoadAsync();
        }

        private async Task InitialLoadAsync()
        {
            var tutors = await _tutorRepo.GetAllAsync();
            Results.Clear();
            foreach (var t in tutors)
            {
                Results.Add(t);
            }
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
            foreach (var t in results)
            {
                Results.Add(t);
            }
        }

        [RelayCommand]
        public void NavigateToBooking(Tutor tutor)
        {
            var student = _currentStudent ?? CurrentSessionService.CurrentUser as Student;

            if (tutor != null && student != null)
            {
                MainView.Instance.NavigateTo(new BookingView(student, tutor));
            }
            else
            {
                MainView.Instance.NavigateTo(new LoginView());
            }
        }
    }
}