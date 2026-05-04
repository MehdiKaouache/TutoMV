using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Projet_Session_Entreprise.Models;
using Projet_Session_Entreprise.Services;

namespace Projet_Session_Entreprise.Views
{
    public partial class ProfileView : UserControl
    {
        private Student? _currentStudent;

        public ProfileView(object user)
        {
            InitializeComponent();

            if (user is Tutor t)
            {
                this.DataContext = t;
                AvailabilityContainer.Visibility = Visibility.Visible;
                colDispos.Width = new GridLength(350);
                ctrlAvailability.Initialize(t);
                LoadAppointments(t.Id, true);
            }
            else if (user is Student s)
            {
                _currentStudent = s;
                this.DataContext = s;
                AvailabilityContainer.Visibility = Visibility.Collapsed;
                colDispos.Width = new GridLength(0);
                LoadAppointments(s.Id, false);
            }

            this.Loaded += (sender, e) => {
                if (_currentStudent != null) CheckAcceptedAppointmentsAndNotify();
            };
        }

        private void LoadAppointments(int userId, bool isTutor)
        {
            using (var db = new AppDbContext())
            {
                if (isTutor)
                    dgAppointments.ItemsSource = db.Appointments.Where(a => a.TutorId == userId).OrderByDescending(a => a.DateRDV).ToList();
                else
                    dgAppointments.ItemsSource = db.Appointments.Where(a => a.StudentId == userId).OrderByDescending(a => a.DateRDV).ToList();
            }
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