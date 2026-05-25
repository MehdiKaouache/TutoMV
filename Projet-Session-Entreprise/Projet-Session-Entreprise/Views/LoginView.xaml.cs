using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Projet_Session_Entreprise.Infrastructure.Services;
using Projet_Session_Entreprise.Core.Models;

namespace Projet_Session_Entreprise.UI.Views
{
    public partial class LoginView : UserControl
    {
        private bool _isPasswordVisible = false;

        public LoginView()
        {
            InitializeComponent();
        }

        private void btnTogglePassword_Click(object sender, RoutedEventArgs e)
        {
            _isPasswordVisible = !_isPasswordVisible;
            if (_isPasswordVisible)
            {
                txtVisiblePassword.Text = txtPassword.Password;
                txtVisiblePassword.Visibility = Visibility.Visible;
                txtPassword.Visibility = Visibility.Collapsed;
                btnTogglePassword.Content = "🙈";
            }
            else
            {
                txtPassword.Password = txtVisiblePassword.Text;
                txtVisiblePassword.Visibility = Visibility.Collapsed;
                txtPassword.Visibility = Visibility.Visible;
                btnTogglePassword.Content = "👁";
            }
        }

        private void txtPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (!_isPasswordVisible) txtVisiblePassword.Text = txtPassword.Password;
        }

        private void txtVisiblePassword_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (_isPasswordVisible) txtPassword.Password = txtVisiblePassword.Text;
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