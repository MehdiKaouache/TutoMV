using System.Linq;
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
            btnAddSubject.Click += BtnAddSubject_Click;
        }

        private void BtnAddSubject_Click(object sender, RoutedEventArgs e)
        {
            var newSub = txtNewSubject.Text?.Trim();
            if (!string.IsNullOrEmpty(newSub))
            {
                // Avoid duplicates
                bool exists = lstSubjects.Items.Cast<ListBoxItem>()
                    .Any(i => string.Equals(i.Content?.ToString(), newSub, System.StringComparison.OrdinalIgnoreCase));

                if (!exists)
                {
                    lstSubjects.Items.Add(new ListBoxItem { Content = newSub });
                }

                txtNewSubject.Clear();
            }
        }

        private void btnSignUp_Click(object sender, RoutedEventArgs e)
        {
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
                        AverageGrade = 75.0,
                        Role = "Étudiant"
                    });
                }
                else
                {
                    // Gather selected subjects (comma separated)
                    var selected = lstSubjects.SelectedItems.Cast<ListBoxItem>()
                        .Select(i => i.Content?.ToString()?.Trim())
                        .Where(s => !string.IsNullOrEmpty(s))
                        .ToList();

                    // If nothing explicitly selected, include all items from the list
                    if (!selected.Any())
                    {
                        selected = lstSubjects.Items.Cast<ListBoxItem>()
                            .Select(i => i.Content?.ToString()?.Trim())
                            .Where(s => !string.IsNullOrEmpty(s))
                            .ToList();
                    }

                    string subjectString = string.Join(", ", selected);

                    db.Tutors.Add(new Tutor
                    {
                        Nom = txtName.Text,
                        Prenom = txtFirstName.Text,
                        DA = txtDA.Text,
                        Password = txtPassword.Password,
                        Subject = subjectString,
                        Availability = "Monday",
                        AverageGrade = 85.0, // default for teacher accounts
                        IsValidated = true,  // teacher-created accounts are validated by default
                        Role = "Enseignant",
                        NumberOfRatings = 0,
                        TotalRatings = 0
                    });
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