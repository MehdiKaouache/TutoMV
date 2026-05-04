using System;
using System.Linq;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Projet_Session_Entreprise.Models;

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
            try
            {
                using (var db = new AppDbContext())
                {
                    var validatedTutors = db.Tutors.Where(t => t.IsValidated).ToList();
                    dgTutors.ItemsSource = validatedTutors;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur lors du chargement des tuteurs : " + ex.Message);
            }
        }

        private void Reserve_Click(object sender, RoutedEventArgs e)
        {
            var tutor = (sender as Button)?.DataContext as Tutor;

            if (tutor != null)
            {
                MainView.Instance.NavigateTo(new BookingView(_student, tutor));
            }
        }
    }
}