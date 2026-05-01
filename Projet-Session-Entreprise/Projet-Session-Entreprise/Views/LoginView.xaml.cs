using System;
using System.Windows;
using Projet_Session_Entreprise.Services;
using Projet_Session_Entreprise.ViewModels;
using Projet_Session_Entreprise.Models;

namespace Projet_Session_Entreprise.Views
{
    public partial class LoginView : Window
    {
        public LoginView()
        {
            InitializeComponent();
            var viewModel = new LoginViewModel(new AuthService());
            DataContext = viewModel;
            txtPassword.PasswordChanged += (s, e) => viewModel.MotDePasse = txtPassword.Password;
        }

        private async void btnConn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string da = txtDA.Text.Trim();
                string password = txtPassword.Password.Trim();

                var auth = new AuthService();
                var user = await auth.LoginAsync(da, password);

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
                else
                {
                    MessageBox.Show("Erreur DA/Password");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur de connexion : " + ex.Message);
            }
        }

        private void btnReg_Click(object sender, RoutedEventArgs e)
        {
            new RoleSelectionView().Show();
            this.Close();
        }
    }
}