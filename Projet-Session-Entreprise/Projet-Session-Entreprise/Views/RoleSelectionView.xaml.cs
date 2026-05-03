using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Projet_Session_Entreprise.Views
{
    public partial class RoleSelectionView : UserControl
    {
        public RoleSelectionView()
        {
            InitializeComponent();
        }

        private void SelectStudent_Click(object sender, MouseButtonEventArgs e)
        {
            MainView.Instance.NavigateTo(new RegisterView("Etudiant"));
        }

        private void SelectTutor_Click(object sender, MouseButtonEventArgs e)
        {
            MainView.Instance.NavigateTo(new TeacherRegisterView());
        }

        private void BackToLogin_Click(object sender, RoutedEventArgs e)
        {
            MainView.Instance.NavigateTo(new LoginView());
        }
    }
}