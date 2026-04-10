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
            btnCancel.Click += BtnCancel_Click;
        }

        private void BtnCancel_Click(object? sender, RoutedEventArgs e)
        {
            var roleSel = new RoleSelectionView();
            // show role selector without setting Owner to this (because we close this)
            roleSel.Show();
            this.Close();
        }

        private void BtnSignUp_Click(object? sender, RoutedEventArgs e)
        {
            // collect selected subjects
            var selected = lstSubjects.SelectedItems.Cast<ListBoxItem>()
                .Select(i => i.Content?.ToString()?.Trim())
                .Where(s => !string.IsNullOrEmpty(s))
                .ToList();

            // if none selected, fallback to the first listed subject to ensure value
            if (!selected.Any() && lstSubjects.Items.Count > 0)
            {
                var first = (lstSubjects.Items[0] as ListBoxItem)?.Content?.ToString();
                if (!string.IsNullOrEmpty(first)) selected.Add(first);
            }

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
                    AverageGrade = 85.0, // default for teacher-created accounts
                    IsValidated = true,  // teacher accounts created here are validated
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