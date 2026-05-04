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

        private void btnProfile_Click(object sender, RoutedEventArgs e)
        {
            // Keep profile behavior so student notification logic runs
            if (CurrentSessionService.CurrentUser is Student s) new ProfileView(s).Show();
            else if (CurrentSessionService.CurrentUser is Tutor t) new ProfileView(t).Show();
        }

        private void btnHome_Click(object sender, RoutedEventArgs e)
        {
            // Do not reference a concrete MainWindow type.
            // Prefer the application's MainWindow if present, otherwise activate any open window.
            var appMain = Application.Current?.MainWindow;
            if (appMain != null)
            {
                if (!appMain.IsVisible) appMain.Show();
                appMain.Activate();
                return;
            }

            var windows = Application.Current?.Windows;
            if (windows != null)
            {
                foreach (Window window in windows)
                {
                    if (window != null)
                    {
                        if (!window.IsVisible) window.Show();
                        window.Activate();
                        return;
                    }
                }
            }

            // Fallback: open a safe entry point (login) if no window is available
            new LoginView().Show();
        }

        private void btnLogInOrOut(object sender, RoutedEventArgs e)
        {
            CurrentSessionService.CurrentUser = null;
            new LoginView().Show();
            Window.GetWindow(this)?.Close();
        }
    }
}