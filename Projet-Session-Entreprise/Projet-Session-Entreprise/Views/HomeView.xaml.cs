using System.Windows;
using System.Windows.Controls;
using Projet_Session_Entreprise.Services;

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
            else if (CurrentSessionService.IsTutor)
            {
                TutorPanel.Visibility = Visibility.Visible;
            }
            else
            {
                StudentPanel.Visibility = Visibility.Visible;
            }
        }

        private void btnRegisterGuest_Click(object sender, RoutedEventArgs e) => MainView.Instance.NavigateTo(new RoleSelectionView());
        private void btnExplore_Click(object sender, RoutedEventArgs e) => MainView.Instance.NavigateTo(new TutorListView(null!));
    }
}