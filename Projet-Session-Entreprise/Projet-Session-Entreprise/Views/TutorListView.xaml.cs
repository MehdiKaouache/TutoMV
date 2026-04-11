using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Projet_Session_Entreprise.Views
{
    public partial class TutorListView : Window
    {
        private Student _student;

        public TutorListView(Student student)
        {
            InitializeComponent();
            _student = student;
            LoadTutors();
        }

        private void LoadTutors()
        {
            using (var db = new AppDbContext())
            {
                dgTutors.ItemsSource = db.Tutors.Where(t => t.IsValidated).ToList();
            }
        }

        private void Reserve_Click(object sender, RoutedEventArgs e)
        {
            var tutor = (sender as Button)?.DataContext as Tutor;
            if (tutor == null) return;

            var result = MessageBox.Show($"Confirmer la réservation avec {tutor.Prenom} {tutor.Nom} ?", "Confirmation", MessageBoxButton.YesNo);

            if (result == MessageBoxResult.Yes)
            {
                using (var db = new AppDbContext())
                {
                    db.Appointments.Add(new Appointment
                    {
                        StudentId = _student.Id,
                        TutorId = tutor.Id,
                        DateRDV = DateTime.Now.AddDays(1)
                    });
                    db.SaveChanges();
                }
                MessageBox.Show("Réservation effectuée avec succès !");
            }
        }
    }
}