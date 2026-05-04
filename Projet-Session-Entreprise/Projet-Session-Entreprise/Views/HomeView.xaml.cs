using System.Windows;
using System.Windows.Controls;
using Projet_Session_Entreprise.Services;
using Projet_Session_Entreprise.Models;

namespace Projet_Session_Entreprise.Views
{
    public partial class HomeView : UserControl
    {
        public HomeView()
        {
            InitializeComponent();
            LoadDashboard();
        }

        private void LoadDashboard()
        {
            GuestPanel.Visibility = Visibility.Collapsed;
            StudentPanel.Visibility = Visibility.Collapsed;
            TutorPanel.Visibility = Visibility.Collapsed;

            if (CurrentSessionService.CurrentUser == null)
            {
                GuestPanel.Visibility = Visibility.Visible;
            }
            else if (CurrentSessionService.CurrentUser is Tutor)
            {
                TutorPanel.Visibility = Visibility.Visible;
            }
            else
            {
                StudentPanel.Visibility = Visibility.Visible;
            }
        }

        private void btnRegisterGuest_Click(object sender, RoutedEventArgs e) => MainView.Instance.NavigateTo(new RoleSelectionView());

        private void btnExplore_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentSessionService.CurrentUser is Student s)
            {
                MainView.Instance.NavigateTo(new TutorListView(s));
            }
            else
            {
                MainView.Instance.NavigateTo(new LoginView());
            }
        }

        private void btnManageRequests_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentSessionService.CurrentUser is Tutor t)
                MainView.Instance.NavigateTo(new ReceivedRequestsView(t));
        }
    }
}