using System;
using System.Windows;
using System.Windows.Controls;
using Projet_Session_Entreprise.Services;
using Projet_Session_Entreprise.Models;
using Projet_Session_Entreprise.Views;

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
            if (isTutor)
            {
                btnSearch.Visibility = Visibility.Collapsed;
                btnBecomeTutor.Visibility = Visibility.Collapsed;
                btnManageRequests.Visibility = Visibility.Visible;
            }
            else
            {
                btnSearch.Visibility = Visibility.Visible;
                btnBecomeTutor.Visibility = Visibility.Visible;
                btnManageRequests.Visibility = Visibility.Collapsed;
            }
        }

        private void btnHome_Click(object sender, RoutedEventArgs e) => MainView.Instance.NavigateTo(new HomeView());

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            // Keep profile behavior so student notification logic runs
            if (CurrentSessionService.CurrentUser is Student s) new ProfileView(s).Show();
            else if (CurrentSessionService.CurrentUser is Tutor t) new ProfileView(t).Show();
        }

        private void btnProfile_Click(object sender, RoutedEventArgs e)
        {
            var user = CurrentSessionService.CurrentUser;
            if (user is Student s) new ProfileView(s).Show();
            else if (user is Tutor t) new ProfileView(t).Show();
        }

        private void btnManageRequests_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentSessionService.CurrentUser is Tutor t)
                MainView.Instance.NavigateTo(new ReceivedRequestsView(t));
        }

        private void btnAppointments_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Le calendrier des rendez-vous sera disponible bientôt.");
        }

        private void btnBecomeTutor_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentSessionService.CurrentUser is Student s)
            {
                var tempTutor = new Tutor { DA = s.DA, Nom = s.Nom, Prenom = s.Prenom, Password = s.Password };
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