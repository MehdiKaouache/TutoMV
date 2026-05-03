using System.Windows;
using System.Windows.Controls;
using Projet_Session_Entreprise.Services;

namespace Projet_Session_Entreprise.Views
{
    public partial class MainView : Window
    {
        public static MainView Instance { get; private set; } = null!;

        public MainView()
        {
            InitializeComponent();
            Instance = this;
            UpdateNavigationMode();
            NavigateTo(new HomeView());
        }

        public void NavigateTo(UserControl view)
        {
            MainContentArea.Content = view;
            UpdateNavigationMode();
        }

        public void UpdateNavigationMode()
        {
            bool isConnected = CurrentSessionService.CurrentUser != null;

            if (isConnected)
            {
                TopNavbar.Visibility = Visibility.Collapsed;
                LeftSidebar.Visibility = Visibility.Visible;
                SidebarCol.Width = new GridLength(280);
                LeftSidebar.UpdateMenu(CurrentSessionService.IsTutor);
            }
            else
            {
                TopNavbar.Visibility = Visibility.Visible;
                LeftSidebar.Visibility = Visibility.Collapsed;
                SidebarCol.Width = new GridLength(0);
            }
        }
    }
}