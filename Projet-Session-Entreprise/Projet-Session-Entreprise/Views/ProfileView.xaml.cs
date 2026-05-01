using System.Windows;
using Projet_Session_Entreprise.ViewModels;
using Projet_Session_Entreprise.Models;

namespace Projet_Session_Entreprise.Views
{
    public partial class ProfileView : Window
    {
        private Student? _currentStudent;


        public ProfileView(Student student)
        {
            InitializeComponent();
            _currentStudent = student;
            DataContext = new ProfileViewModel(student);
        }

        public ProfileView(Tutor tutor)
        {
            InitializeComponent();
            DataContext = new ProfileViewModel(tutor);
        }

        private void SearchTutor_Click(object sender, RoutedEventArgs e)
        {
            if (_currentStudent != null)
            {
                new TutorListView(_currentStudent).Show();
            }
        }
   
    }
}