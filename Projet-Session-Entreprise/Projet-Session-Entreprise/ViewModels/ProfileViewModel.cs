using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using System.Windows;
using System.Linq;
using Projet_Session_Entreprise.Models;

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

        public ObservableCollection<Appointment> MyAppointments { get; set; } = new ObservableCollection<Appointment>();

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
                }
                else if (_student != null)
                {
                    var appts = db.Appointments.Where(a => a.StudentId == _student.Id).ToList();
                    foreach (var a in appts) MyAppointments.Add(a);
                }
            }
        }
    }
}