using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Projet_Session_Entreprise.Data;
using Projet_Session_Entreprise.Models;
using Projet_Session_Entreprise.Repositories.Interfaces;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace Projet_Session_Entreprise.ViewModels
{
    public partial class ProfileViewModel : ObservableObject
    {
        private readonly IAppointmentRepository _appointmentRepo;
        private Student? _student;
        private Tutor? _tutor;

        [ObservableProperty] private bool _isTutor;
        [ObservableProperty] private string _dA = "";
        [ObservableProperty] private string _nom = "";
        [ObservableProperty] private string _prenom = "";
        [ObservableProperty] private string _role = "";
        [ObservableProperty] private string _statusMessage = "";

        [ObservableProperty] private Visibility _studentSectionVisibility = Visibility.Collapsed;
        [ObservableProperty] private Visibility _tutorSectionVisibility = Visibility.Collapsed;
        [ObservableProperty] private Visibility _reviewSectionVisibility = Visibility.Collapsed;
        [ObservableProperty] private Visibility _detailsVisibility = Visibility.Collapsed;

        [ObservableProperty] private int _totalSeances;
        [ObservableProperty] private int _heuresCompletees;
        [ObservableProperty] private string _moyenneStats = "N/A";

        [ObservableProperty] private int _rating;
        [ObservableProperty] private string _comment = "";

        [ObservableProperty] private Appointment? _selectedAppointment;

        public ObservableCollection<Appointment> MyAppointments { get; set; } = new();
        public ObservableCollection<Review> Reviews { get; set; } = new();

        public ProfileViewModel(Student student)
        {
            _appointmentRepo = App.AppointmentRepo;
            _student = student;
            IsTutor = false;
            DA = student.DA;
            Nom = student.Nom;
            Prenom = student.Prenom;
            Role = student.Role;
            StudentSectionVisibility = Visibility.Visible;
            LoadData();
        }

        public ProfileViewModel(Tutor tutor) : this(tutor, App.AppointmentRepo) { }

        public ProfileViewModel(Tutor tutor, IAppointmentRepository appointmentRepo)
        {
            _appointmentRepo = appointmentRepo;
            _tutor = tutor;
            IsTutor = true;
            DA = tutor.DA;
            Nom = tutor.Nom;
            Prenom = tutor.Prenom;
            Role = tutor.Role;
            TutorSectionVisibility = Visibility.Visible;
            LoadData();
        }

        public void LoadData()
        {
            using (var db = new AppDbContext())
            {
                MyAppointments.Clear();
                Reviews.Clear();

                if (IsTutor && _tutor != null)
                {
                    var appts = db.Appointments.Where(a => a.TutorId == _tutor.Id).ToList();
                    foreach (var a in appts) MyAppointments.Add(a);

                    var revs = db.Reviews.Where(r => r.TutorId == _tutor.Id).ToList();
                    foreach (var r in revs) Reviews.Add(r);

                    TotalSeances = appts.Count(a => a.Status != null && (a.Status.ToLower() == "complété" || a.Status.ToLower() == "accepté"));
                    HeuresCompletees = TotalSeances;
                    MoyenneStats = revs.Any() ? Math.Round(revs.Average(r => r.Rating), 1).ToString() + " / 5" : "N/A";
                }
                else if (_student != null)
                {
                    var appts = db.Appointments.Where(a => a.StudentId == _student.Id).ToList();
                    foreach (var a in appts) MyAppointments.Add(a);

                    TotalSeances = appts.Count(a => a.Status != null && (a.Status.ToLower() == "complété" || a.Status.ToLower() == "accepté"));
                    HeuresCompletees = TotalSeances;
                    MoyenneStats = "-";
                }
            }
        }

        partial void OnSelectedAppointmentChanged(Appointment? value)
        {
            if (value != null)
            {
                DetailsVisibility = Visibility.Visible;

                if (!IsTutor && (value.Status?.ToLower() == "complété" || value.Status?.ToLower() == "accepté"))
                {
                    ReviewSectionVisibility = Visibility.Visible;
                }
                else
                {
                    ReviewSectionVisibility = Visibility.Collapsed;
                }
            }
            else
            {
                DetailsVisibility = Visibility.Collapsed;
                ReviewSectionVisibility = Visibility.Collapsed;
            }
        }

        [RelayCommand]
        public void AddReview()
        {
            if (SelectedAppointment == null || Rating < 1 || Rating > 5)
            {
                StatusMessage = "Veuillez entrer une note de 1 à 5.";
                return;
            }

            try
            {
                using (var db = new AppDbContext())
                {
                    var review = new Review
                    {
                        TutorId = SelectedAppointment.TutorId,
                        Rating = Rating,
                        Comment = Comment
                    };

                    db.Reviews.Add(review);
                    db.SaveChanges();

                    StatusMessage = "Avis envoyé avec succès !";
                    Rating = 0;
                    Comment = "";
                    ReviewSectionVisibility = Visibility.Collapsed;
                    LoadData();
                }
            }
            catch (Exception ex)
            {
                StatusMessage = "Erreur BD : " + (ex.InnerException?.Message ?? ex.Message);
            }
        }

        [RelayCommand]
        public void AcceptAppointment(Appointment appt)
        {
            UpdateAppointmentStatus(appt, "Accepté");
        }

        [RelayCommand]
        public void RefuseAppointment(Appointment appt)
        {
            UpdateAppointmentStatus(appt, "Refusé");
        }

        private void UpdateAppointmentStatus(Appointment appt, string status)
        {
            if (appt == null) return;
            using (var db = new AppDbContext())
            {
                var existing = db.Appointments.Find(appt.Id);
                if (existing != null)
                {
                    existing.Status = status;
                    db.SaveChanges();
                    LoadData();
                }
            }
        }

        public bool AppointmentExists(DateTime date)
        {
            using (var db = new AppDbContext())
            {
                if (_student != null)
                {
                    return db.Appointments.Any(a => a.DateRDV == date && a.StudentId == _student.Id);
                }

                if (_tutor != null)
                {
                    return db.Appointments.Any(a => a.DateRDV == date && a.TutorId == _tutor.Id);
                }
            }
            return false;
        }
    }
}