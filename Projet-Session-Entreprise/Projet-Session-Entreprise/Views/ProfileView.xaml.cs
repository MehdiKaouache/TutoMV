using System.Windows;
using Projet_Session_Entreprise.Models;
using Projet_Session_Entreprise.Services;
using Projet_Session_Entreprise.ViewModels;

namespace Projet_Session_Entreprise.Views
{
    public partial class ProfileView : Window
    {
        private Student? _currentStudent;

        public ProfileView(Student student)
        {
            InitializeComponent();
            _currentStudent = student;
            DataContext = new ProfileViewModel(student);

            this.Loaded += (sender, e) =>
            {
                if (_currentStudent != null) CheckAcceptedAppointmentsAndNotify();
            };
        }

        public ProfileView(Tutor tutor)
        {
            InitializeComponent();
            DataContext = new ProfileViewModel(tutor);
        }

        private void SearchTutor_Click(object sender, RoutedEventArgs e)
        {
            if (_currentStudent != null)
                MainView.Instance.NavigateTo(new TutorListView(_currentStudent));
        }

        private void CheckAcceptedAppointmentsAndNotify()
        {
            if (_currentStudent == null) return;
            try
            {
                using (var db = new AppDbContext())
                {
                    var appts = db.Appointments.ToList();
                    var tutors = db.Tutors.ToList();
                    NotificationService.ShowAcceptedAppointmentsForStudent(_currentStudent, appts, tutors);
                }
            }
            catch { }
        }
    }
}
