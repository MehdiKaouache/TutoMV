using System;
using System.Windows;
using Projet_Session_Entreprise.ViewModels;

namespace Projet_Session_Entreprise.Views
{
    public partial class ProfileView : Window
    {
        public ProfileView(Student student)
        {
            InitializeComponent();
            
            DataContext = new ProfileViewModel(student);
        }

        public ProfileView(Tutor tutor)
        {
            InitializeComponent();
            
            DataContext = new ProfileViewModel(tutor);
        }
    }
}