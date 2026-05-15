using CommunityToolkit.Mvvm.ComponentModel;
using Projet_Session_Entreprise.Models;
using Projet_Session_Entreprise.Repositories.Interfaces;
using System.Collections.ObjectModel;

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

        public ObservableCollection<Appointment> MyAppointments { get; set; } = new();

        public ProfileViewModel(Student student) : this(student, App.AppointmentRepo) { }
        public ProfileViewModel(Tutor tutor) : this(tutor, App.AppointmentRepo) { }

        public ProfileViewModel(Student student, IAppointmentRepository appointmentRepo)
        {
            _student = student;
            _appointmentRepo = appointmentRepo;
            _dA = student.DA; _nom = student.Nom; _prenom = student.Prenom; _role = student.Role;
            _ = LoadDataAsync();
        }

        public ProfileViewModel(Tutor tutor, IAppointmentRepository appointmentRepo)
        {
            _tutor = tutor; _isTutor = true;
            _appointmentRepo = appointmentRepo;
            _dA = tutor.DA; _nom = tutor.Nom; _prenom = tutor.Prenom; _role = tutor.Role;
            _ = LoadDataAsync();
        }

        public async Task LoadDataAsync()
        {
            MyAppointments.Clear();
            var appointments = IsTutor ?
                await _appointmentRepo.GetByTutorIdAsync(_tutor!.Id) :
                await _appointmentRepo.GetByStudentIdAsync(_student!.Id);

            if (appointments != null)
            {
                foreach (var a in appointments.OrderByDescending(x => x.DateRDV))
                {
                    MyAppointments.Add(a);
                }
            }
        }
    }
}