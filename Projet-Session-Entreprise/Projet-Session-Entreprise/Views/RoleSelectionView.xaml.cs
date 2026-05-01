using System.Windows;
using System.Windows.Input;

namespace Projet_Session_Entreprise.Views
{
    public partial class RoleSelectionView : Window
    {
        public RoleSelectionView()
        {
            InitializeComponent();
        }

        private void SelectStudent_Click(object sender, MouseButtonEventArgs e)
        {
            new RegisterView("Etudiant").Show();
            this.Close();
        }

        private void SelectTutor_Click(object sender, MouseButtonEventArgs e)
        {
            new TeacherRegisterView().Show();
            this.Close();
        }

        private void BackToLogin_Click(object sender, RoutedEventArgs e)
        {
            new LoginView().Show();
            this.Close();
        }
    }
}