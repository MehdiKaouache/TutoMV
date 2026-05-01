using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Windows;
using Projet_Session_Entreprise.Models;

namespace Projet_Session_Entreprise.ViewModels
{
    public partial class ProfileViewModel : ObservableObject
    {
        private Student? _student;
        private Tutor? _tutor;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(StudentSectionVisibility))]
        [NotifyPropertyChangedFor(nameof(TutorSectionVisibility))]
        private bool _isTutor;

        [ObservableProperty] private string _dA = "";
        [ObservableProperty] private string _nom = "";
        [ObservableProperty] private string _prenom = "";
        [ObservableProperty] private string _availability = "";
        [ObservableProperty] private string _statusMessage = "";
        [ObservableProperty] private ObservableCollection<Review> _reviews = new ObservableCollection<Review>();

        public Visibility StudentSectionVisibility => IsTutor ? Visibility.Collapsed : Visibility.Visible;
        public Visibility TutorSectionVisibility => IsTutor ? Visibility.Visible : Visibility.Collapsed;

        public ObservableCollection<Tutor> Tutors { get; set; } = new ObservableCollection<Tutor>();
        public ObservableCollection<Appointment> MyAppointments { get; set; } = new ObservableCollection<Appointment>();
        public ObservableCollection<string> AvailabilityChoices { get; } = new ObservableCollection<string> { "Lundi", "Mardi", "Mercredi", "Jeudi", "Vendredi", "Samedi", "Dimanche" };

        public ProfileViewModel(Student student)
        {
            _student = student;
            IsTutor = false;
            DA = student.DA;
            Nom = student.Nom;
            Prenom = student.Prenom;
            LoadData();
        }

        public ProfileViewModel(Tutor tutor)
        {
            _tutor = tutor;
            IsTutor = true;
            DA = tutor.DA;
            Nom = tutor.Nom;
            Prenom = tutor.Prenom;
            Availability = tutor.Availability;
            LoadData();
        }

        private void LoadData()
        {
            using (var db = new AppDbContext())
            {
                if (!IsTutor && _student != null)
                {
                    var tutorsList = db.Tutors.Where(t => t.IsValidated).ToList();
                    Tutors.Clear();
                    foreach (var t in tutorsList) Tutors.Add(t);

                    var appts = db.Appointments.Where(a => a.StudentId == _student.Id).ToList();
                    MyAppointments.Clear();
                    foreach (var a in appts) MyAppointments.Add(a);
                }
                else if (IsTutor && _tutor != null)
                {
                    var appts = db.Appointments.Where(a => a.TutorId == _tutor.Id).ToList();
                    MyAppointments.Clear();
                    foreach (var a in appts) MyAppointments.Add(a);

                    var tutorReviews = db.Reviews.Where(r => r.TutorId == _tutor.Id).ToList();
                    Reviews.Clear();
                    foreach (var r in tutorReviews) Reviews.Add(r);
                }
            }
        }

        [RelayCommand]
        private void SaveAvailability()
        {
            if (_tutor == null) return;
            using (var db = new AppDbContext())
            {
                var t = db.Tutors.FirstOrDefault(x => x.Id == _tutor.Id);
                if (t != null)
                {
                    t.Availability = Availability;
                    db.SaveChanges();
                    StatusMessage = "Disponibilités mises à jour.";
                }
            }
        }
    }
}