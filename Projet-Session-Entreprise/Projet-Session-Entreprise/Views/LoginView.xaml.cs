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

                txtError.Visibility = Visibility.Collapsed;

                var user = await App.AuthService.LoginAsync(da, password);

                if (user != null)
                {
                    CurrentSessionService.CurrentUser = user;
                    MainView.Instance.UpdateNavigationMode();
                    MainView.Instance.NavigateTo(new HomeView());
                }
                else
                {
                    txtError.Text = "DA ou mot de passe invalide.";
                    txtError.Visibility = Visibility.Visible;
                }
            }
            catch (Exception ex)
            {
                txtError.Text = "Erreur : " + ex.Message;
                txtError.Visibility = Visibility.Visible;
            }
        }

        private void BtnRegister_Click(object sender, RoutedEventArgs e)
        {
            MainView.Instance.NavigateTo(new RoleSelectionView());
        }
    }
}