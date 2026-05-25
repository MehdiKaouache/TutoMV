using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Projet_Session_Entreprise.Infrastructure.Services;
using Projet_Session_Entreprise.Core.Models;

namespace Projet_Session_Entreprise.UI.Views
{
    public partial class RegisterView : UserControl
    {
        private string _role;
        private bool _isPasswordVisible = false;

        public RegisterView(string role)
        {
            InitializeComponent();
            _role = role;
            lblTitle.Text = _role == "Etudiant" ? "Inscription Étudiant" : "Inscription Enseignant";
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

        private void btnCancel_Click(object sender, RoutedEventArgs e) => MainView.Instance.NavigateTo(new LoginView());

        private void ShowError(string message)
        {
            txtError.Text = message;
            txtError.Visibility = Visibility.Visible;
        }

        private async void btnSignUp_Click(object sender, RoutedEventArgs e)
        {
            string nom = txtName.Text.Trim();
            string prenom = txtFirstName.Text.Trim();
            string da = txtDA.Text.Trim();
            string password = txtPassword.Password;

            txtError.Visibility = Visibility.Collapsed;

            if (string.IsNullOrWhiteSpace(nom) || string.IsNullOrWhiteSpace(prenom) || string.IsNullOrWhiteSpace(da) || !double.TryParse(txtGPA.Text, out double gpa))
            {
                ShowError("Veuillez remplir tous les champs correctement.");
                return;
            }

            if (da.Length != 7 || !da.All(char.IsDigit))
            {
                ShowError("Le numéro de DA doit contenir exactement 7 chiffres.");
                return;
            }

            if (password.Length < 8)
            {
                ShowError("Le mot de passe doit contenir au moins 8 caractères.");
                return;
            }

            bool success = await App.AuthService.RegisterAsync(nom, prenom, da, _role, password, gpa);

            if (success)
            {
                txtError.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Green);
                ShowError("Compte créé avec succès ! Redirection...");
                await System.Threading.Tasks.Task.Delay(1000);
                MainView.Instance.NavigateTo(new LoginView());
            }
            else
            {
                ShowError("Ce DA est déjà utilisé.");
            }
        }
    }
}