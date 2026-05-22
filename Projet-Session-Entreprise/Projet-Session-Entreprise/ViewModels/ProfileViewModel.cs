using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Projet_Session_Entreprise.Models;
using Projet_Session_Entreprise.Repositories.Interfaces;
using Projet_Session_Entreprise.Services.Interfaces;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Projet_Session_Entreprise.ViewModels
{
    public partial class ProfileViewModel : ObservableObject
    {
        private readonly IAppointmentRepository _appointmentRepo;
        private readonly ITutorRepository _tutorRepo;
        private readonly IReviewRepository _reviewRepo;
        private Student? _student;
        private Tutor? _tutor;

        [ObservableProperty] private bool _isTutor;
        [ObservableProperty] private string _dA = "";
        [ObservableProperty] private string _nom = "";
        [ObservableProperty] private string _prenom = "";
        [ObservableProperty] private string _role = "";
        [ObservableProperty] private string _availability = "";
        [ObservableProperty] private string _statusMessage = "";
        [ObservableProperty] private Appointment? _selectedAppointment;
        [ObservableProperty] private int _rating;
        [ObservableProperty] private string _comment = "";

        public ObservableCollection<Appointment> MyAppointments { get; set; } = new();
        public ObservableCollection<Review> Reviews { get; set; } = new();

        public ObservableCollection<string> AvailabilityChoices { get; } = new()
        {
            "Lundi", "Mardi", "Mercredi", "Jeudi", "Vendredi", "Samedi", "Dimanche"
        };

        public Visibility TutorSectionVisibility => IsTutor ? Visibility.Visible : Visibility.Collapsed;
        public Visibility StudentSectionVisibility => !IsTutor ? Visibility.Visible : Visibility.Collapsed;

        public ProfileViewModel(Student student) : this(student, App.AppointmentRepo, App.TutorRepo, App.ReviewRepo) { }
        public ProfileViewModel(Tutor tutor) : this(tutor, App.AppointmentRepo, App.TutorRepo, App.ReviewRepo) { }

        public ProfileViewModel(Student student, IAppointmentRepository appointmentRepo, ITutorRepository tutorRepo, IReviewRepository reviewRepo)
        {
            _student = student;
            _appointmentRepo = appointmentRepo;
            _tutorRepo = tutorRepo;
            _reviewRepo = reviewRepo;
            IsTutor = false;
            DA = student.DA; Nom = student.Nom; Prenom = student.Prenom; Role = student.Role;
            _ = LoadDataAsync();
        }

        public ProfileViewModel(Tutor tutor, IAppointmentRepository appointmentRepo, ITutorRepository tutorRepo, IReviewRepository reviewRepo)
        {
            _tutor = tutor;
            _appointmentRepo = appointmentRepo;
            _tutorRepo = tutorRepo;
            _reviewRepo = reviewRepo;
            IsTutor = true;
            DA = tutor.DA; Nom = tutor.Nom; Prenom = tutor.Prenom; Role = tutor.Role;
            Availability = tutor.Availability;
            _ = LoadDataAsync();
        }

        public async Task LoadDataAsync()
        {
            MyAppointments.Clear();
            Reviews.Clear();

            if (IsTutor && _tutor != null)
            {
                var appts = await _appointmentRepo.GetByTutorIdAsync(_tutor.Id);
                foreach (var a in appts.OrderByDescending(x => x.DateRDV)) MyAppointments.Add(a);

                var tutorReviews = await _reviewRepo.GetByTutorIdAsync(_tutor.Id);
                foreach (var r in tutorReviews) Reviews.Add(r);
            }
            else if (_student != null)
            {
                var appts = await _appointmentRepo.GetByStudentIdAsync(_student.Id);
                foreach (var a in appts.OrderByDescending(x => x.DateRDV)) MyAppointments.Add(a);
            }
        }

        [RelayCommand]
        private async Task SaveAvailabilityAsync()
        {
            if (_tutor == null) return;
            var t = await _tutorRepo.GetByIdAsync(_tutor.Id);
            if (t != null)
            {
                t.Availability = Availability;
                _tutorRepo.Update(t);
                await _tutorRepo.SaveChangesAsync();
                StatusMessage = "Disponibilités mises à jour.";
            }
        }

        [RelayCommand]
        private async Task AcceptAppointmentAsync(Appointment appointment)
        {
            if (_tutor == null || appointment == null) return;
            appointment.Status = "Accepté";
            _appointmentRepo.Update(appointment);
            await _appointmentRepo.SaveChangesAsync();
            await LoadDataAsync();
        }

        [RelayCommand]
        private async Task RefuseAppointmentAsync(Appointment appointment)
        {
            if (_tutor == null || appointment == null) return;
            appointment.Status = "Refusé";
            _appointmentRepo.Update(appointment);
            await _appointmentRepo.SaveChangesAsync();
            await LoadDataAsync();
        }

        [RelayCommand]
        private async Task AddReviewAsync()
        {
            if (_student == null || SelectedAppointment == null) return;
            await _reviewRepo.AddAsync(new Review
            {
                TutorId = SelectedAppointment.TutorId,
                StudentId = _student.Id,
                Rating = Rating,
                Comment = Comment
            });
            await _reviewRepo.SaveChangesAsync();
            Rating = 0; Comment = "";
            await LoadDataAsync();
        }
    }
}