using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Projet_Session_Entreprise.Models;
using Projet_Session_Entreprise.Data;

namespace Projet_Session_Entreprise.Views
{
    public partial class TeacherRegisterView : UserControl
    {
        public TeacherRegisterView()
        {
            InitializeComponent();
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e) => MainView.Instance.NavigateTo(new RoleSelectionView());

        private void BtnSignUp_Click(object sender, RoutedEventArgs e)
        {
            string nom = txtName.Text.Trim();
            string prenom = txtFirstName.Text.Trim();
            string da = txtDA.Text.Trim();
            string password = txtPassword.Password;

            if (string.IsNullOrEmpty(nom) || string.IsNullOrEmpty(prenom) || string.IsNullOrEmpty(da) || !double.TryParse(txtGPA.Text, out double gpa))
            {
                MessageBox.Show("Tous les champs sont obligatoires.");
                return;
            }

            if (da.Length != 7 || !da.All(char.IsDigit))
            {
                MessageBox.Show("Le DA doit contenir exactement 7 chiffres.");
                return;
            }

            if (password.Length < 8)
            {
                MessageBox.Show("Le mot de passe doit contenir au moins 8 caractères.");
                return;
            }

            if (gpa < 80)
            {
                MessageBox.Show("Une moyenne de 80% est requise pour devenir tuteur.");
                return;
            }

            try
            {
                using (var db = new AppDbContext())
                {
                    if (db.Students.Any(s => s.DA == da) || db.Tutors.Any(t => t.DA == da))
                    {
                        MessageBox.Show("Ce DA est déjà associé à un compte.");
                        return;
                    }

                    var newTutor = new Tutor
                    {
                        Nom = nom,
                        Prenom = prenom,
                        DA = da,
                        Password = BCrypt.Net.BCrypt.HashPassword(password),
                        AverageGrade = gpa,
                        Role = "Tuteur",
                        IsValidated = false
                    };

                    db.Tutors.Add(newTutor);
                    db.SaveChanges();
                    MainView.Instance.NavigateTo(new RequeteRoleTuteurView(newTutor));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur : " + ex.Message);
            }
        }
    }
}
