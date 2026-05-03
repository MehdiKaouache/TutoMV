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
            if (CurrentSessionService.CurrentUser is Student s)
                MainView.Instance.NavigateTo(new TutorListView(s));
        }

        private void btnBecomeTutor_Click(object sender, RoutedEventArgs e)
        {
            MainView.Instance.NavigateTo(new RequeteRoleTuteurView(null!));
        }

        private void btnProfile_Click(object sender, RoutedEventArgs e)
        {
            var user = CurrentSessionService.CurrentUser;
            if (user != null)
            {
                MainView.Instance.NavigateTo(new ProfileView(user));
            }
        }

        private void btnAppointments_Click(object sender, RoutedEventArgs e) => MessageBox.Show("Chargement...");

        private void btnLogInOrOut(object sender, RoutedEventArgs e)
        {
            CurrentSessionService.CurrentUser = null;
            MainView.Instance.NavigateTo(new LoginView());
        }
    }
}