using System;
using System.Windows;
using System.Windows.Controls;
using Projet_Session_Entreprise.Services;
using Projet_Session_Entreprise.Models;

namespace Projet_Session_Entreprise.Views
{
    public partial class SidebarControlView : UserControl
    {
        public SidebarControlView()
        {
            InitializeComponent();
        }

        public void UpdateMenu(bool isTutor)
        {
            btnSearch.Visibility = isTutor ? Visibility.Collapsed : Visibility.Visible;
            btnBecomeTutor.Visibility = isTutor ? Visibility.Collapsed : Visibility.Visible;
            btnManageRequests.Visibility = isTutor ? Visibility.Visible : Visibility.Collapsed;
        }

        private void btnHome_Click(object sender, RoutedEventArgs e) => MainView.Instance.NavigateTo(new HomeView());

        private void btnSearch_Click(object sender, RoutedEventArgs e) => MainView.Instance.NavigateTo(new TutorListView());

        private void btnProfile_Click(object sender, RoutedEventArgs e)
        {
            var user = CurrentSessionService.CurrentUser;
            if (user != null)
            {
                MainView.Instance.NavigateTo(new ProfileView(user));
            }
        }

        private void btnManageRequests_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentSessionService.CurrentUser is Tutor t)
            {
                MainView.Instance.NavigateTo(new ReceivedRequestsView(t));
            }
        }

        private void btnAppointments_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Le calendrier sera disponible au Sprint 4.");
        }

        private void btnBecomeTutor_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentSessionService.CurrentUser is Student s)
            {
                var tempTutor = new Tutor
                {
                    DA = s.DA,
                    Nom = s.Nom,
                    Prenom = s.Prenom,
                    Password = s.Password
                };
                MainView.Instance.NavigateTo(new RequeteRoleTuteurView(tempTutor));
            }
        }

        private void btnSettings_Click(object sender, RoutedEventArgs e)
        {
            MainView.Instance.NavigateTo(new SettingsView());
        }

        private void btnLogInOrOut(object sender, RoutedEventArgs e)
        {
            CurrentSessionService.CurrentUser = null;
            MainView.Instance.UpdateNavigationMode();
            MainView.Instance.NavigateTo(new LoginView());
        }
    }
}