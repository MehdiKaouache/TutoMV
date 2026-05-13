using CommunityToolkit.Mvvm.ComponentModel;
using Projet_Session_Entreprise.Models;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

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
        [ObservableProperty] private string _role = "";

        public ObservableCollection<Appointment> MyAppointments { get; set; } = new ObservableCollection<Appointment>();

        public ProfileViewModel(Student student)
        {
            _student = student;
            _isTutor = false;
            _dA = student.DA;
            _nom = student.Nom;
            _prenom = student.Prenom;
            _role = student.Role;
            _ = LoadDataAsync();
        }

        public ProfileViewModel(Tutor tutor)
        {
            _tutor = tutor;
            _isTutor = true;
            _dA = tutor.DA;
            _nom = tutor.Nom;
            _prenom = tutor.Prenom;
            _role = tutor.Role;
            _ = LoadDataAsync();
        }

        public async Task LoadDataAsync()
        {
            try
            {
                MyAppointments.Clear();
                System.Collections.Generic.IEnumerable<Appointment> appointments;

                if (IsTutor && _tutor != null)
                {
                    appointments = await App.AppointmentRepo.GetByTutorIdAsync(_tutor.Id);
                }
                else if (_student != null)
                {
                    appointments = await App.AppointmentRepo.GetByStudentIdAsync(_student.Id);
                }
                else
                {
                    return;
                }

                if (appointments != null)
                {
                    foreach (var a in appointments.OrderByDescending(x => x.DateRDV))
                    {
                        MyAppointments.Add(a);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Erreur de chargement des rendez-vous : " + ex.Message);
            }
        }

        public async Task<bool> AppointmentExistsAsync(DateTime date)
        {
            var appointments = IsTutor && _tutor != null
                ? await App.AppointmentRepo.GetByTutorIdAsync(_tutor.Id)
                : await App.AppointmentRepo.GetByStudentIdAsync(_student?.Id ?? 0);

            return appointments != null && appointments.Any(a => a.DateRDV == date);
        }
    }
}