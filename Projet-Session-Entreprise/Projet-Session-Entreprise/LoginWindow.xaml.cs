using Projet_Session_Entreprise.Services;
using System.Windows;

namespace Projet_Session_Entreprise
{
    public partial class LoginWindow : Window
    {
        private readonly AuthService authService = new AuthService();

        public LoginWindow()
        {
            InitializeComponent();
        }

        private async void Login_Click(object sender, RoutedEventArgs e)
        {
            string da = txtDA.Text;
            string password = txtPassword.Password;

            var auth = new AuthService();

            bool success = await auth.LoginAsync(da, password);

            if (success)
            {
                new MainWindow().Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Mauvais DA ou mot de passe");
            }
        }

       
    }
}