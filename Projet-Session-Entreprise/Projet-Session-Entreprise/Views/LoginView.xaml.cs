using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Projet_Session_Entreprise.Services;
using Projet_Session_Entreprise.Models;

namespace Projet_Session_Entreprise.Views
{
    public partial class LoginView : UserControl
    {
        public LoginView()
        {
            InitializeComponent();
        }

        private async void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string da = txtDA.Text.Trim();
                string password = txtPassword.Password.Trim();

                var auth = new AuthService();
                var user = await auth.LoginAsync(da, password);

                if (user != null)
                {
                    // Directly open the student profile so notification logic runs without test prompt
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
                    MessageBox.Show("DA ou mot de passe invalide.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur : " + ex.Message);
            }
        }

        private void BtnRegister_Click(object sender, RoutedEventArgs e)
        {
            MainView.Instance.NavigateTo(new RoleSelectionView());
        }
    }
}