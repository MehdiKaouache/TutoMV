using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Projet_Session_Entreprise.Infrastructure.Data;
using Projet_Session_Entreprise.Core.Models;
using Projet_Session_Entreprise.UI.Views;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using Projet_Session_Entreprise.Infrastructure.Services;

namespace Projet_Session_Entreprise.UI.ViewModels
{
    public partial class ProfileViewModel : ObservableObject
    {
        private Student? _student;
        private Tutor? _tutor;

        [ObservableProperty] private bool _isTutor;
        [ObservableProperty] private bool _isStudent;
        [ObservableProperty] private string _dA = "";
        [ObservableProperty] private string _nom = "";
        [ObservableProperty] private string _prenom = "";
        [ObservableProperty] private string _role = "";
        [ObservableProperty] private string _statusMessage = "";

        [ObservableProperty] private string _gpa = "";
        [ObservableProperty] private string _matiere = "";
        [ObservableProperty] private string _statutValidation = "";

        [ObservableProperty] private Visibility _reviewSectionVisibility = Visibility.Collapsed;
        [ObservableProperty] private Visibility _detailsVisibility = Visibility.Collapsed;

        [ObservableProperty] private int _totalSeances;
        [ObservableProperty] private int _heuresCompletees;
        [ObservableProperty] private string _moyenneStats = "N/A";

        [ObservableProperty] private int _rating;
        [ObservableProperty] private string _comment = "";

        [ObservableProperty] private bool _canEditGpa;
        [ObservableProperty] private string _newGpaInput = "";
        [ObservableProperty] private string _gpaStatusMessage = "";

        [ObservableProperty] private Appointment? _selectedAppointment;

        public ObservableCollection<Appointment> MyAppointments { get; set; } = new();

        public ProfileViewModel(Student student)
        {
            _student = student;
            IsTutor = false;
            IsStudent = true;
            DA = student.DA;
            Nom = student.Nom;
            Prenom = student.Prenom;
            Role = student.Role;
            Gpa = student.GPA.ToString("0.0");

            var currentUser = CurrentSessionService.CurrentUser;
            if (currentUser is Tutor t)
            {
                using (var db = new AppDbContext())
                {
                    CanEditGpa = db.Appointments.Any(a => a.TutorId == t.Id && a.StudentId == student.Id && a.Status != "Refusé");
                }
            }
            else
            {
                CanEditGpa = false;
            }

            LoadData();
        }

        public ProfileViewModel(Tutor tutor)
        {
            _tutor = tutor;
            IsTutor = true;
            IsStudent = false;
            DA = tutor.DA;
            Nom = tutor.Nom;
            Prenom = tutor.Prenom;
            Role = tutor.Role;
            Matiere = tutor.Subject;
            StatutValidation = tutor.IsValidated ? "Certifié ✅" : "En attente de certification";
            CanEditGpa = false;
            LoadData();
        }

        public void LoadData()
        {
            using (var db = new AppDbContext())
            {
                MyAppointments.Clear();

                if (IsTutor && _tutor != null)
                {
                    var appts = db.Appointments.Where(a => a.TutorId == _tutor.Id).ToList();
                    foreach (var a in appts) MyAppointments.Add(a);

                    var revs = db.Reviews.Where(r => r.TutorId == _tutor.Id).ToList();

                    TotalSeances = appts.Count(a => a.Status != null && (a.Status.ToLower() == "complété" || a.Status.ToLower() == "terminé"));
                    HeuresCompletees = TotalSeances;
                    MoyenneStats = revs.Any() ? Math.Round(revs.Average(r => r.Rating), 1).ToString() + " / 5" : "N/A";
                }
                else if (_student != null)
                {
                    var appts = db.Appointments.Where(a => a.StudentId == _student.Id).ToList();
                    foreach (var a in appts) MyAppointments.Add(a);

                    TotalSeances = appts.Count(a => a.Status != null && (a.Status.ToLower() == "complété" || a.Status.ToLower() == "terminé"));
                    HeuresCompletees = TotalSeances;
                    MoyenneStats = "-";
                }
            }
        }

        [RelayCommand]
        public void UpdateStudentGPA()
        {
            if (_student != null && CanEditGpa)
            {
                if (double.TryParse(NewGpaInput, out double val))
                {
                    using (var db = new AppDbContext())
                    {
                        var s = db.Students.Find(_student.Id);
                        if (s != null)
                        {
                            s.GPA = val;
                            db.SaveChanges();
                            Gpa = val.ToString("0.0");
                            GpaStatusMessage = "✅ Moyenne mise à jour avec succès !";
                            NewGpaInput = "";
                        }
                    }
                }
                else
                {
                    GpaStatusMessage = "❌ Format invalide. Entrez un nombre.";
                }
            }
        }

        partial void OnSelectedAppointmentChanged(Appointment? value)
        {
            if (value != null)
            {
                DetailsVisibility = Visibility.Visible;
                if (!IsTutor && (value.Status?.ToLower() == "complété" || value.Status?.ToLower() == "terminé"))
                    ReviewSectionVisibility = Visibility.Visible;
                else
                    ReviewSectionVisibility = Visibility.Collapsed;
            }
            else
            {
                DetailsVisibility = Visibility.Collapsed;
                ReviewSectionVisibility = Visibility.Collapsed;
            }
        }

        [RelayCommand]
        public void CompleteAppointment(Appointment appt)
        {
            if (appt == null || _tutor == null) return;
            try
            {
                using (var db = new AppDbContext())
                {
                    var dbAppt = db.Appointments.Find(appt.Id);
                    if (dbAppt != null)
                    {
                        dbAppt.Status = "Terminé";
                        db.CompletedAppointments.Add(new CompletedAppointment
                        {
                            AppointmentId = dbAppt.Id,
                            StudentId = dbAppt.StudentId,
                            TutorId = dbAppt.TutorId,
                            CompletedDate = DateTime.Now
                        });
                        db.SaveChanges();
                    }
                }
                StatusMessage = "Séance marquée comme terminée !";
                LoadData();
            }
            catch (Exception ex)
            {
                StatusMessage = "Erreur BD : " + (ex.InnerException?.Message ?? ex.Message);
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
                        AppointmentId = SelectedAppointment.Id,
                        TutorId = SelectedAppointment.TutorId,
                        StudentId = SelectedAppointment.StudentId,
                        Rating = Rating,
                        Comment = Comment
                    };
                    db.Reviews.Add(review);
                    db.SaveChanges();
                }

                StatusMessage = "Avis envoyé avec succès !";
                Rating = 0;
                Comment = "";
                ReviewSectionVisibility = Visibility.Collapsed;
                LoadData();
            }
            catch (Exception ex)
            {
                StatusMessage = "Erreur BD : " + (ex.InnerException?.Message ?? ex.Message);
            }
        }

        [RelayCommand]
        public void OpenChat()
        {
            if (SelectedAppointment != null)
            {
                MainView.Instance.NavigateTo(new MessageListView());
            }
        }
    }
}