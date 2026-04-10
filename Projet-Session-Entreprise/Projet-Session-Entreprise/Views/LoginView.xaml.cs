using System;
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
            try
            {
                string da = txtDA.Text.Trim();
                string password = txtPassword.Password.Trim();

                var auth = new AuthService();
                var user = await auth.LoginAsync(da, password);

                Window? nextWindow = null;

                if (user is Student s)
                {
                    nextWindow = new ProfileView(s);
                }
                else if (user is Tutor t)
                {
                    if (t.IsValidated)
                    {
                        nextWindow = new ProfileView(t);
                    }
                    else
                    {
                        nextWindow = new RequeteRoleTuteurView(t, new AppDbContext());
                    }
                }
                else
                {
                    MessageBox.Show("Erreur DA/Password");
                    return;
                }

                Application.Current.MainWindow = nextWindow;
                nextWindow.Show();
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur lors de la connexion : " + ex.ToString());
            }
        }

        private void btnReg_Click(object sender, RoutedEventArgs e)
        {
            var roleSel = new RoleSelectionView();
            // do NOT set roleSel.Owner = this; — closing this would close roleSel too
            roleSel.Show();
            this.Close();
        }
    }
}