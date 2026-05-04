using System.Text;
using System.Windows;
using System.Linq;
using Projet_Session_Entreprise.ViewModels;
using Projet_Session_Entreprise.Models;
using Projet_Session_Entreprise.Services;

namespace Projet_Session_Entreprise.Views
{
    public partial class ProfileView : UserControl
    {
        private Tutor? _currentTutor;
        private Student? _currentStudent;

        public ProfileView(Student student)
        {
            InitializeComponent();
            _currentStudent = student;
            DataContext = new ProfileViewModel(student);

            // Use the centralized notification helper
            CheckAcceptedAppointmentsAndNotify();
        }

        public ProfileView(Tutor tutor)
        {
            InitializeComponent();
            DataContext = new ProfileViewModel(tutor);
        }

        private void BtnShowAdd_Click(object sender, RoutedEventArgs e)
        {
            AjouterSlotArea.Visibility = Visibility.Visible;
            btnShowAdd.Visibility = Visibility.Collapsed;
        }

        private void BtnSaveNewSlot_Click(object sender, RoutedEventArgs e)
        {
            if (_currentStudent != null)
            {
                new TutorListView(_currentStudent).Show();
            }
        }

        private void CheckAcceptedAppointmentsAndNotify()
        {
            if (_currentStudent == null) return;

            using (var db = new AppDbContext())
            {
                var appts = db.Appointments.ToList();
                var tutors = db.Tutors.ToList();

                NotificationService.ShowAcceptedAppointmentsForStudent(_currentStudent, appts, tutors);
            }
        }
    }
}