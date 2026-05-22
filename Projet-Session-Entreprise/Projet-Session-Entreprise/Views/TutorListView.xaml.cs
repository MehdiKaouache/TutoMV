using System;
using System.Windows;
using System.Windows.Controls;
using Projet_Session_Entreprise.Models;
using Projet_Session_Entreprise.ViewModels;
using Projet_Session_Entreprise.Services;

namespace Projet_Session_Entreprise.Views
{
    public partial class TutorListView : UserControl
    {
        private readonly Student? _currentStudent;

        public TutorListView()
        {
            InitializeComponent();
            this.DataContext = new TutorListViewModel();
        }

        public TutorListView(Student student)
        {
            InitializeComponent();
            _currentStudent = student;
            this.DataContext = new TutorListViewModel(student);
        }

        private void Reserve_Click(object sender, RoutedEventArgs e)
        {
            var tutor = (sender as Button)?.DataContext as Tutor;
            var student = _currentStudent ?? CurrentSessionService.CurrentUser as Student;

            if (tutor != null && student != null)
            {
                MainView.Instance.NavigateTo(new BookingView(student, tutor));
            }
            else if (student == null)
            {
                MessageBox.Show("Veuillez vous connecter en tant qu'étudiant pour réserver.");
            }
        }
    }
}