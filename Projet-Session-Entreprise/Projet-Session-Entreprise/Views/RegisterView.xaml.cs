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
            lblTitle.Text = role == "Etudiant" ? "Inscription Étudiant" : "Inscription Enseignant";
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            MainView.Instance.NavigateTo(new LoginView());
        }

        private void btnSignUp_Click(object sender, RoutedEventArgs e)
        {
            string nom = txtName.Text.Trim();
            string prenom = txtFirstName.Text.Trim();
            string da = txtDA.Text.Trim();
            string gpaText = txtGPA.Text.Trim();
            string password = txtPassword.Password;

            if (string.IsNullOrWhiteSpace(nom) || string.IsNullOrWhiteSpace(prenom) ||
                string.IsNullOrWhiteSpace(da) || string.IsNullOrWhiteSpace(gpaText) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Erreur : Tous les champs sont obligatoires.");
                return;
            }

            if (da.Length != 7 || !da.All(char.IsDigit))
            {
                MessageBox.Show("Erreur : Le DA doit contenir exactement 7 chiffres.");
                return;
            }

            if (password.Length < 8)
            {
                MessageBox.Show("Erreur : Le mot de passe doit faire au moins 8 caractères.");
                return;
            }

            if (!double.TryParse(gpaText, out double gpa))
            {
                MessageBox.Show("Erreur : La moyenne doit être un nombre valide.");
                return;
            }



            using (var db = new AppDbContext())
            {
                try
                {
                    bool alreadyExists = db.Students.Any(s => s.DA == da) || db.Tutors.Any(t => t.DA == da);
                    if (alreadyExists)
                    {
                        MessageBox.Show("Ce numéro de DA est déjà associé à un compte.");
                        return;
                    }

                    if (_role == "Etudiant")
                    {
                        db.Students.Add(new Student { Nom = nom, Prenom = prenom, DA = da, Password = password, AverageGrade = gpa, Role = "Étudiant" });
                    }
                    else
                    {
                        if (gpa < 80)
                        {
                            MessageBox.Show("Refusé : Un tuteur doit avoir au moins 80% de moyenne.");
                            return;
                        }
                        db.Tutors.Add(new Tutor { Nom = nom, Prenom = prenom, DA = da, Password = password, AverageGrade = gpa, Role = "Tuteur" });
                    }

                    db.SaveChanges();
                    MessageBox.Show("Compte créé avec succès !");
                    MainView.Instance.NavigateTo(new LoginView());
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erreur DB : " + ex.Message);
                }
            }
        }
    }
}