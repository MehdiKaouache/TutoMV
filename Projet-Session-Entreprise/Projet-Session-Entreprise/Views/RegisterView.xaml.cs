using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Projet_Session_Entreprise.Services;
using Projet_Session_Entreprise.Models;

namespace Projet_Session_Entreprise.Views
{
    public partial class RegisterView : UserControl
    {
        private string _role;

        public RegisterView(string role)
        {
            InitializeComponent();
            _role = role;
            lblTitle.Text = _role == "Etudiant" ? "Inscription Étudiant" : "Inscription Enseignant";
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e) => MainView.Instance.NavigateTo(new LoginView());

        private async void btnSignUp_Click(object sender, RoutedEventArgs e)
        {
            string nom = txtName.Text.Trim();
            string prenom = txtFirstName.Text.Trim();
            string da = txtDA.Text.Trim();
            string password = txtPassword.Password;

            if (string.IsNullOrWhiteSpace(nom) || string.IsNullOrWhiteSpace(prenom) || string.IsNullOrWhiteSpace(da) || !double.TryParse(txtGPA.Text, out double gpa))
            {
                MessageBox.Show("Veuillez remplir tous les champs.");
                return;
            }

            if (da.Length != 7 || !da.All(char.IsDigit))
            {
                MessageBox.Show("Le numéro de DA doit contenir exactement 7 chiffres.");
                return;
            }

            if (password.Length < 8)
            {
                MessageBox.Show("Le mot de passe doit contenir au moins 8 caractères.");
                return;
            }

            bool success = await App.AuthService.RegisterAsync(nom, prenom, da, _role, password, gpa);

            if (success)
            {
                MessageBox.Show("Compte créé avec succès !");
                MainView.Instance.NavigateTo(new LoginView());
            }
            else
            {
                MessageBox.Show("Ce DA est déjà utilisé.");
            }
        }
    }
}