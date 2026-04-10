using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Projet_Session_Entreprise.Models;
using System.Collections.ObjectModel;
using System.Windows;
using System.Linq;

namespace Projet_Session_Entreprise.ViewModels
{
    public partial class ProfileViewModel : ObservableObject
    {
        private Student? _student;
        private Tutor? _tutor;

        [ObservableProperty] private bool _isTutor;
        [ObservableProperty] private string _dA = "";
        [ObservableProperty] private string _nom = "";
        [ObservableProperty] private string _prenom = "";
        [ObservableProperty] private string _availability = "";
        [ObservableProperty] private string _statusMessage = "";

        public Visibility StudentSectionVisibility => IsTutor ? Visibility.Collapsed : Visibility.Visible;
        public Visibility TutorSectionVisibility => IsTutor ? Visibility.Visible : Visibility.Collapsed;

        public ObservableCollection<Tutor> Tutors { get; set; } = new ObservableCollection<Tutor>();
        public ObservableCollection<Review> Reviews { get; set; } = new ObservableCollection<Review>();

        public ObservableCollection<int> RatingChoices { get; } = new ObservableCollection<int> { 1, 2, 3, 4, 5 };
        public ObservableCollection<string> AvailabilityChoices { get; } =
            new ObservableCollection<string> { "Lundi", "Mardi", "Mercredi", "Jeudi", "Vendredi" };

        [ObservableProperty] private Tutor? _selectedTutor;
        [ObservableProperty] private int _selectedRating = 5;
        [ObservableProperty] private string _selectedAvailability = "Lundi";

        public ProfileViewModel(Student student)
        {
            _student = student;
            IsTutor = false;
            DA = student.DA;
            Nom = student.Nom;
            Prenom = student.Prenom;
        }

        public ProfileViewModel(Tutor tutor)
        {
            _tutor = tutor;
            IsTutor = true;
            DA = tutor.DA;
            Nom = tutor.Nom;
            Prenom = tutor.Prenom;
            Availability = tutor.Availability;
        }

        [RelayCommand]
        private void AddRating()
        {
            if (SelectedTutor == null) return;
            StatusMessage = "Note ajoutée.";
        }

        [RelayCommand]
        private void SaveAvailability()
        {
            if (_tutor == null) return;
            Availability = SelectedAvailability;
            StatusMessage = "Dispos mises à jour.";
        }
    }
}