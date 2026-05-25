using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Projet_Session_Entreprise.Core.Models;
using Projet_Session_Entreprise.Core.Interfaces;
using Projet_Session_Entreprise.Infrastructure.Services;
using Projet_Session_Entreprise.UI.Views;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace Projet_Session_Entreprise.UI.ViewModels
{
    public partial class TutorListViewModel : ObservableObject
    {
        private readonly ITutorRepository _tutorRepo;
        private Student? _currentStudent;
        private List<Tutor> _allTutors = new();

        [ObservableProperty] private string _searchText = "";
        [ObservableProperty] private string _selectedSubject = "Toutes les matières";
        [ObservableProperty] private string _selectedRating = "Toutes les notes";

        public ObservableCollection<string> Subjects { get; } = new();
        public ObservableCollection<string> Ratings { get; } = new() { "Toutes les notes", "⭐ 4.0 et +", "⭐ 4.5 et +" };
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
            _allTutors = tutors.ToList();

            Subjects.Clear();
            var allCegepSubjects = new List<string> {
                "Toutes les matières",
                "Mathématiques", "Informatique", "Français", "Anglais",
                "Physique", "Chimie", "Biologie", "Psychologie",
                "Philosophie", "Histoire", "Administration",
                "Économie", "Arts Visuels", "Géographie"
            };

            foreach (var sub in allCegepSubjects) Subjects.Add(sub);

            SelectedSubject = "Toutes les matières";
            ApplyFilters();
        }

        partial void OnSelectedSubjectChanged(string value) => ApplyFilters();
        partial void OnSelectedRatingChanged(string value) => ApplyFilters();

        [RelayCommand]
        public void ApplyFilters()
        {
            var filtered = _allTutors.AsEnumerable();

            if (!string.IsNullOrWhiteSpace(SearchText))
            {
                var lowerSearch = SearchText.ToLower();
                filtered = filtered.Where(t =>
                    t.Nom.ToLower().Contains(lowerSearch) ||
                    t.Prenom.ToLower().Contains(lowerSearch));
            }

            if (SelectedSubject != "Toutes les matières" && !string.IsNullOrEmpty(SelectedSubject))
            {
                filtered = filtered.Where(t => t.Subject.Contains(SelectedSubject));
            }

            if (SelectedRating == "⭐ 4.0 et +")
            {
                filtered = filtered.Where(t => t.AverageRating >= 4.0);
            }
            else if (SelectedRating == "⭐ 4.5 et +")
            {
                filtered = filtered.Where(t => t.AverageRating >= 4.5);
            }

            Results.Clear();
            foreach (var t in filtered)
            {
                Results.Add(t);
            }
        }

        [RelayCommand]
        public void NavigateToBooking(Tutor tutor)
        {
            var student = _currentStudent ?? CurrentSessionService.CurrentUser as Student;

            if (tutor != null && student != null)
                MainView.Instance.NavigateTo(new BookingView(student, tutor));
            else
                MainView.Instance.NavigateTo(new LoginView());
        }
    }
}