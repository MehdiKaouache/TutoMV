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

        private void btnProfile_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentSessionService.CurrentUser is Student s) new ProfileView(s).Show();
            else if (CurrentSessionService.CurrentUser is Tutor t) new ProfileView(t).Show();
        }

        private void btnHome_Click(object sender, RoutedEventArgs e)
        {
            foreach (Window window in Application.Current.Windows)
            {
                if (window is MainWindow) { window.Focus(); return; }
            }
            new MainWindow().Show();
        }

        private void btnLogInOrOut(object sender, RoutedEventArgs e)
        {
            CurrentSessionService.CurrentUser = null;
            new LoginView().Show();
            Window.GetWindow(this)?.Close();
        }
    }
}