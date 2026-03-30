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
            using (var db = new AppDbContext())
            {
                string role = (cmbUserType.SelectedItem as ComboBoxItem)?.Content.ToString();

                if (role == "Étudiant")
                {
                    db.Students.Add(new Student { Nom = txtName.Text, Prenom = txtFirstName.Text, DA = txtDA.Text, Password = txtPassword.Password, AverageGrade = 75.0 });
                }
                else
                {
                    db.Tutors.Add(new Tutor { Nom = txtName.Text, Prenom = txtFirstName.Text, DA = txtDA.Text, Password = txtPassword.Password });
                }
                db.SaveChanges();
                MessageBox.Show("Compte créé !");
                LoginView login = new LoginView();
                login.Show();
                this.Close();
            }
        }
    }
}