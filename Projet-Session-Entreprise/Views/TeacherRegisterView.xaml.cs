using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Projet_Session_Entreprise.Views
{
    public partial class TeacherRegisterView : Window
    {
        public TeacherRegisterView()
        {
            InitializeComponent();
            btnSignUp.Click += BtnSignUp_Click;
            btnCancel.Click += (s, e) => { var roleSel = new RoleSelectionView(); roleSel.Owner = this.Owner; roleSel.Show(); this.Close(); };
        }

        private void BtnSignUp_Click(object sender, RoutedEventArgs e)
        {
            using (var db = new AppDbContext())
            {
                var selected = lstSubjects.SelectedItems.Cast<ListBoxItem>()
                    .Select(i => i.Content?.ToString()?.Trim())
                    .Where(s => !string.IsNullOrEmpty(s))
                    .ToList();

                // If none explicitly selected, fallback to all listed subjects to ensure a value
                if (!selected.Any())
                {
                    selected = lstSubjects.Items.Cast<ListBoxItem>()
                        .Select(i => i.Content?.ToString()?.Trim())
                        .Where(s => !string.IsNullOrEmpty(s))
                        .ToList();
                }

                string subjectString = string.Join(", ", selected);
                string availability = (cmbAvailability.SelectedItem as ComboBoxItem)?.Content?.ToString() ?? "Lundi";

                db.Tutors.Add(new Tutor
                {
                    Nom = txtName.Text,
                    Prenom = txtFirstName.Text,
                    DA = txtDA.Text,
                    Password = txtPassword.Password,
                    Subject = subjectString,
                    Availability = availability,
                    AverageGrade = 85.0, // default for teacher accounts
                    IsValidated = true,  // teacher-created accounts are validated by default
                    Role = "Enseignant",
                    NumberOfRatings = 0,
                    TotalRatings = 0
                });

                db.SaveChanges();
            }

            MessageBox.Show("Compte enseignant créé !");
            var login = new LoginView();
            login.Show();
            this.Close();
        }
    }
}