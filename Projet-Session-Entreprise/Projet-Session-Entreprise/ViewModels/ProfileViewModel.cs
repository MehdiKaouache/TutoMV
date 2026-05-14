using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Projet_Session_Entreprise.Models;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

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
        [ObservableProperty] private ObservableCollection<Review> _reviews = new ObservableCollection<Review>();
        [ObservableProperty] private ObservableCollection<CompletedAppointment> _completedAppointments = new ObservableCollection<CompletedAppointment>();
        [ObservableProperty] private Appointment? _selectedAppointment;
        [ObservableProperty] private CompletedAppointment? _selectedCompletedAppointment;
        [ObservableProperty] private int _rating;
        [ObservableProperty] private string _comment = "";

        public ObservableCollection<Appointment> MyAppointments { get; set; } = new ObservableCollection<Appointment>();

        public ObservableCollection<string> AvailabilityChoices { get; } = new ObservableCollection<string>
        {
            "Lundi", "Mardi", "Mercredi", "Jeudi", "Vendredi", "Samedi", "Dimanche"
        };

        public Visibility TutorSectionVisibility => IsTutor ? Visibility.Visible : Visibility.Collapsed;
        public Visibility StudentSectionVisibility => !IsTutor ? Visibility.Visible : Visibility.Collapsed;

        public ProfileViewModel(Student student)
        {
            _student = student;
            _tutor = null;
            IsTutor = false;
            DA = student.DA;
            Nom = student.Nom;
            Prenom = student.Prenom;
            LoadData();
        }

        public ProfileViewModel(Tutor tutor)
        {
            _tutor = tutor;
            _student = null;
            IsTutor = true;
            DA = tutor.DA;
            Nom = tutor.Nom;
            Prenom = tutor.Prenom;
            Availability = tutor.Availability;
            LoadData();
        }

        public void LoadData()
        {
            using (var db = new AppDbContext())
            {
                if (!IsTutor && _student != null)
                {
                    var appts = db.Appointments.Where(a => a.StudentId == _student.Id).ToList();
                    MyAppointments.Clear();
                    foreach (var a in appts) MyAppointments.Add(a);

                    var completed = db.CompletedAppointments.Where(c => c.StudentId == _student.Id).ToList();
                    CompletedAppointments.Clear();
                    foreach (var c in completed) CompletedAppointments.Add(c);
                }
                else if (IsTutor && _tutor != null)
                {
                    var appts = db.Appointments.Where(a => a.TutorId == _tutor.Id).ToList();
                    MyAppointments.Clear();
                    foreach (var a in appts) MyAppointments.Add(a);

                    var completed = db.CompletedAppointments.Where(c => c.TutorId == _tutor.Id).ToList();
                    CompletedAppointments.Clear();
                    foreach (var c in completed) CompletedAppointments.Add(c);

                    var tutorReviews = db.Reviews.Where(r => r.TutorId == _tutor.Id).ToList();
                    Reviews.Clear();
                    foreach (var r in tutorReviews) Reviews.Add(r);
                }
            }
        }

        public bool AppointmentExists(DateTime date)
        {
            using (var db = new AppDbContext())
            {
                if (_student != null)
                {
                    if (db.Appointments.Any(a => a.DateRDV == date && a.StudentId == _student.Id))
                        return true;
                }
                if (_tutor != null)
                {
                    return db.Appointments.Any(a => a.DateRDV == date && a.TutorId == _tutor.Id);
                }
            }
            return false;
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

        [RelayCommand]
        private void AcceptAppointment(Appointment appointment)
        {
            if (_tutor == null)
            {
                MessageBox.Show("Seul un tuteur peut accepter un rendez-vous.");
                return;
            }

            if (appointment == null)
            {
                MessageBox.Show("Rendez-vous invalide.");
                return;
            }

            using (var db = new AppDbContext())
            {
                var appointmentDb = db.Appointments.FirstOrDefault(a =>
                    a.Id == appointment.Id && a.TutorId == _tutor.Id);

                if (appointmentDb == null)
                {
                    MessageBox.Show("Rendez-vous introuvable.");
                    return;
                }

                if (appointmentDb.Status == "Terminé")
                {
                    MessageBox.Show("Impossible de modifier une séance terminée.");
                    return;
                }

                appointmentDb.Status = "Accepté";
                db.SaveChanges();
                MessageBox.Show("Rendez-vous accepté.");
                LoadData();
            }
        }

        [RelayCommand]
        private void RefuseAppointment(Appointment appointment)
        {
            if (_tutor == null)
            {
                MessageBox.Show("Seul un tuteur peut refuser un rendez-vous.");
                return;
            }

            if (appointment == null)
            {
                MessageBox.Show("Rendez-vous invalide.");
                return;
            }

            using (var db = new AppDbContext())
            {
                var appointmentDb = db.Appointments.FirstOrDefault(a =>
                    a.Id == appointment.Id && a.TutorId == _tutor.Id);

                if (appointmentDb == null)
                {
                    MessageBox.Show("Rendez-vous introuvable.");
                    return;
                }

                if (appointmentDb.Status == "Terminé")
                {
                    MessageBox.Show("Impossible de modifier une séance terminée.");
                    return;
                }

                appointmentDb.Status = "Refusé";
                db.SaveChanges();
                MessageBox.Show("Rendez-vous refusé.");
                LoadData();
            }
        }

        [RelayCommand]
        private void CompleteAppointment()
        {
            if (_tutor == null)
            {
                MessageBox.Show("Seul un tuteur peut terminer une séance.");
                return;
            }

            if (SelectedAppointment == null)
            {
                MessageBox.Show("Sélectionnez une séance.");
                return;
            }

            using (var db = new AppDbContext())
            {
                var appointment = db.Appointments.FirstOrDefault(a =>
                    a.Id == SelectedAppointment.Id && a.TutorId == _tutor.Id);

                if (appointment == null)
                {
                    MessageBox.Show("Séance introuvable.");
                    return;
                }

                if (appointment.Status != "Accepté")
                {
                    MessageBox.Show($"Impossible de terminer cette séance. Statut actuel : {appointment.Status}");
                    return;
                }

                bool dejaTerminee = db.CompletedAppointments.Any(c => c.AppointmentId == appointment.Id);
                if (dejaTerminee)
                {
                    appointment.Status = "Terminé";
                    db.SaveChanges();
                    MessageBox.Show("Cette séance était déjà archivée. Le statut a été corrigé.");
                    LoadData();
                    return;
                }

                appointment.Status = "Terminé";
                db.CompletedAppointments.Add(new CompletedAppointment
                {
                    AppointmentId = appointment.Id,
                    StudentId = appointment.StudentId,
                    TutorId = appointment.TutorId,
                    CompletedDate = DateTime.Now
                });

                db.SaveChanges();
                MessageBox.Show("Séance terminée et archivée.");
                LoadData();
            }
        }

        [RelayCommand]
        private void AddReview()
        {
            if (_student == null)
            {
                MessageBox.Show("Seul un élève peut noter une séance.");
                return;
            }

            if (SelectedCompletedAppointment == null)
            {
                MessageBox.Show("Sélectionnez une séance terminée.");
                return;
            }

            if (Rating < 1 || Rating > 5)
            {
                MessageBox.Show("La note doit être entre 1 et 5.");
                return;
            }

            using (var db = new AppDbContext())
            {
                var completed = db.CompletedAppointments.FirstOrDefault(c =>
                    c.Id == SelectedCompletedAppointment.Id && c.StudentId == _student.Id);

                if (completed == null)
                {
                    MessageBox.Show("Séance terminée introuvable.");
                    return;
                }

                bool dejaNotee = db.Reviews.Any(r =>
                    r.CompletedAppointmentId == completed.Id && r.StudentId == _student.Id);

                if (dejaNotee)
                {
                    MessageBox.Show("Cette séance a déjà été notée.");
                    return;
                }

                var tutor = db.Tutors.FirstOrDefault(t => t.Id == completed.TutorId);
                if (tutor == null)
                {
                    MessageBox.Show("Tuteur introuvable.");
                    return;
                }

                db.Reviews.Add(new Review
                {
                    CompletedAppointmentId = completed.Id,
                    TutorId = tutor.Id,
                    StudentId = _student.Id,
                    Rating = Rating,
                    Comment = Comment
                });

                tutor.NumberOfRatings += 1;
                tutor.TotalRatings += Rating;

                db.SaveChanges();
                MessageBox.Show("Avis ajouté et statistiques du tuteur mises à jour.");

                Rating = 0;
                Comment = "";
                LoadData();
            }
        }
    }
}
