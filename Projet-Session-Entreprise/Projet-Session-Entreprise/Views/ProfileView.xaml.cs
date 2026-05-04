using Projet_Session_Entreprise.Models;
using Projet_Session_Entreprise.Services;
using Projet_Session_Entreprise.ViewModels;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

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
                DateTime date = GetDateTimeAppointment();
                var vm = DataContext as ProfileViewModel;

                if (vm != null && vm.AppointmentExists(date))
                {
                    MessageBox.Show("Vous avez déjà un rendez-vous à ce moment");
                    return;
                }
                MainView.Instance.NavigateTo(new TutorListView(_currentStudent)); //tutorListView est un UserControl, pas un window, so .Show() marches pas
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


        private DateTime GetDateTimeAppointment() //pour retourner les dates en DateTime pour comparer les appointments avec leurs daates
        {
            string day = (cmbDay.SelectedItem as ComboBoxItem)?.Content.ToString();
            string time = (cmbTime.SelectedItem as ComboBoxItem)?.Content.ToString();

            //prends la date de today, check si c'est le même jours que l'user à choisi, sinon, va au next day et recommence.
            //Ex: je pick mercredi prochain et on est jeudi, ça va parse les jours jusqu'a hit mercredi (donc +6 jours)
            DateTime date = DateTime.Today;
            while (date.ToString("dddd", new System.Globalization.CultureInfo("fr-CA")).ToLower() != day.ToLower())
            {
                date = date.AddDays(1);
            }

            TimeSpan ts = TimeSpan.Parse(time);
            return date.Date.Add(ts);
        }
    }
}