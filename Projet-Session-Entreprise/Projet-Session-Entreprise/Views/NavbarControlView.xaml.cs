using System.Windows;
using System.Windows.Controls;

namespace Projet_Session_Entreprise.Views
{
    public partial class NavbarControlView : UserControl
    {
        public NavbarControlView()
        {
            InitializeComponent();
        }

        private void BtnHome_Click(object sender, RoutedEventArgs e)
        {
            MainView.Instance.NavigateTo(new HomeView());
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            MainView.Instance.NavigateTo(new LoginView());
        }

        private void BtnRegister_Click(object sender, RoutedEventArgs e)
        {
            MainView.Instance.NavigateTo(new RoleSelectionView());
        }
    }
}