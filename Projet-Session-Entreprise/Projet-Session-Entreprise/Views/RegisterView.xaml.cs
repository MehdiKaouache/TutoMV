using System.Windows;
using System.Windows.Controls;

namespace Projet_Session_Entreprise.Views
{
    public partial class RegisterView : Window
    {
        public RegisterView()
        {
            InitializeComponent();
            btnSignUp.Click += btnSignUp_Click;
        }

        private void btnSignUp_Click(object sender, RoutedEventArgs e)
        {
            if (!double.TryParse(txtGPA.Text, out double gpa))
            {
                MessageBox.Show("Moyenne invalide.");
                return;
            }

            using (var db = new AppDbContext())
            {
                string role = (cmbUserType.SelectedItem as ComboBoxItem)?.Content?.ToString() ?? "";

                if (role == "Étudiant")
                {
                    db.Students.Add(new Student
                    {
                        Nom = txtName.Text,
                        Prenom = txtFirstName.Text,
                        DA = txtDA.Text,
                        Password = txtPassword.Password,
                        AverageGrade = gpa,
                        Role = "Étudiant"
                    });
                }
                else
                {
                    if (gpa < 80)
                    {
                        MessageBox.Show("Moyenne de 80% requise pour être tuteur.");
                        return;
                    }

                    db.Tutors.Add(new Tutor
                    {
                        Nom = txtName.Text,
                        Prenom = txtFirstName.Text,
                        DA = txtDA.Text,
                        Password = txtPassword.Password,
                        AverageGrade = gpa,
                        IsValidated = true,
                        Role = "Enseignant"
                    });
                }

                db.SaveChanges();
                MessageBox.Show("Compte créé !");
                new LoginView().Show();
                this.Close();
            }
        }
    }
}