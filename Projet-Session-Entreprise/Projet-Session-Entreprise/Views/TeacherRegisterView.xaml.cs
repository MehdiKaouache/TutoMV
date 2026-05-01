using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Projet_Session_Entreprise.Models;

namespace Projet_Session_Entreprise.Views
{
    public partial class TeacherRegisterView : Window
    {
        public TeacherRegisterView()
        {
            InitializeComponent();
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            new RoleSelectionView().Show();
            this.Close();
        }

        private void BtnSignUp_Click(object sender, RoutedEventArgs e)
        {
            string da = txtDA.Text.Trim();
            string password = txtPassword.Password;

            if (string.IsNullOrWhiteSpace(txtName.Text) || string.IsNullOrWhiteSpace(txtFirstName.Text) ||
                string.IsNullOrWhiteSpace(da) || string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(txtGPA.Text))
            {
                MessageBox.Show("Tous les champs sont requis.");
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

            if (!double.TryParse(txtGPA.Text, out double gpa))
            {
                MessageBox.Show("Veuillez saisir une moyenne valide.");
                return;
            }

            if (gpa < 80)
            {
                MessageBox.Show("Une moyenne de 80% est requise pour devenir tuteur.");
                return;
            }

            var selected = lstSubjects.SelectedItems.Cast<ListBoxItem>()
                .Select(i => i.Content?.ToString())
                .ToList();

            if (selected.Count == 0)
            {
                MessageBox.Show("Veuillez sélectionner au moins une matière.");
                return;
            }

            string subjectString = string.Join(", ", selected);
            string availability = (cmbAvailability.SelectedItem as ComboBoxItem)?.Content?.ToString() ?? "Lundi";

            using (var db = new AppDbContext())
            {
                db.Tutors.Add(new Tutor
                {
                    Nom = txtName.Text,
                    Prenom = txtFirstName.Text,
                    DA = da,
                    Password = password,
                    Subject = subjectString,
                    Availability = availability,
                    AverageGrade = gpa,
                    IsValidated = true,
                    Role = "Tuteur"
                });

                db.SaveChanges();
            }

            MessageBox.Show("Compte tuteur créé avec succès !");
            new LoginView().Show();
            this.Close();
        }
    }
}