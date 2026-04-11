using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Collections.Generic;
using Projet_Session_Entreprise;

namespace Projet_Session_Entreprise.Views
{
    public partial class TeacherRegisterView : Window
    {
        public TeacherRegisterView()
        {
            InitializeComponent();
            btnSignUp.Click += BtnSignUp_Click;
            btnCancel.Click += BtnCancel_Click;
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            new RoleSelectionView().Show();
            this.Close();
        }

        private void BtnSignUp_Click(object sender, RoutedEventArgs e)
        {
            if (!double.TryParse(txtGPA.Text, out double gpa))
            {
                MessageBox.Show("Veuillez saisir une moyenne valide.");
                return;
            }

            if (gpa < 80)
            {
                MessageBox.Show("Désolé, une moyenne de 80% est requise pour devenir tuteur.");
                return;
            }

            var selected = lstSubjects.SelectedItems.Cast<ListBoxItem>()
                .Select(i => i.Content?.ToString()?.Trim())
                .Where(s => !string.IsNullOrEmpty(s))
                .ToList();

            string subjectString = string.Join(", ", selected);
            string availability = (cmbAvailability.SelectedItem as ComboBoxItem)?.Content?.ToString() ?? "Lundi";

            using (var db = new AppDbContext())
            {
                db.Tutors.Add(new Tutor
                {
                    Nom = txtName.Text,
                    Prenom = txtFirstName.Text,
                    DA = txtDA.Text,
                    Password = txtPassword.Password,
                    Subject = subjectString,
                    Availability = availability,
                    AverageGrade = gpa,
                    IsValidated = true,
                    Role = "Enseignant"
                });

                db.SaveChanges();
            }

            MessageBox.Show("Compte tuteur validé et créé !");
            new LoginView().Show();
            this.Close();
        }
    }
}