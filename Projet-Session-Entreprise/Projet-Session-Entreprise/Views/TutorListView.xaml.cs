using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Projet_Session_Entreprise.Models;
using System.Threading.Tasks;

namespace Projet_Session_Entreprise.Views
{
    public partial class TutorListView : UserControl
    {
        public TutorListView()
        {
            InitializeComponent();
            _ = LoadTutorsAsync();
        }

        private async Task LoadTutorsAsync()
        {
            try
            {
                if (App.TutorRepo == null) return;

                var tutors = await App.TutorRepo.GetAllAsync();
                dgTutors.ItemsSource = tutors.ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur de chargement : " + ex.Message);
            }
        }

        private async void BtnSearch_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string query = txtSearch.Text.Trim();

                var results = string.IsNullOrWhiteSpace(query)
                    ? await App.TutorRepo.GetAllAsync()
                    : await App.TutorRepo.SearchTutorsAsync(query);

                dgTutors.ItemsSource = results.ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur recherche : " + ex.Message);
            }
        }

        private void Reserve_Click(object sender, RoutedEventArgs e)
        {
            var tutor = (sender as Button)?.DataContext as Tutor;
            var student = Projet_Session_Entreprise.Services.CurrentSessionService.CurrentUser as Student;

            if (tutor != null && student != null)
            {
                MainView.Instance.NavigateTo(new BookingView(student, tutor));
            }
            else if (student == null)
            {
                MessageBox.Show("Veuillez vous connecter en tant qu'étudiant.");
            }
        }
    }
}