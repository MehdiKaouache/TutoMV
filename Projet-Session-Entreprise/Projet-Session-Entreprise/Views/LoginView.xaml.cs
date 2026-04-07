using System.Linq;
using System.Windows;
using Projet_Session_Entreprise.Services;
using Projet_Session_Entreprise.Views;

namespace Projet_Session_Entreprise
{
    public partial class LoginView : Window
    {
        public LoginView()
        {
            InitializeComponent();
        }

        private async void btnConn_Click(object sender, RoutedEventArgs e)
        {
            var auth = new AuthService();
            var user = await auth.LoginAsync(txtDA.Text, txtPassword.Password);

            if (user is Student s)
            {
                new ProfileView(s).Show();
                this.Close();
            }
            else if (user is Tutor t)
            {
                new RequeteRoleTuteurView(t, new AppDbContext()).Show();
                this.Close();
            }
            else MessageBox.Show("Erreur DA/Password");
        }

        private void btnReg_Click(object sender, RoutedEventArgs e)
        {
            var reg = new Views.RegisterView();
            reg.Show();
            this.Close();
        }
    }
}