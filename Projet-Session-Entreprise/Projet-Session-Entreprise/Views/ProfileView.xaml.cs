using System.Windows;
using System.Windows.Controls;
using Projet_Session_Entreprise.ViewModels;
using Projet_Session_Entreprise.Models;
using Projet_Session_Entreprise.Services;

namespace Projet_Session_Entreprise.Views
{
    public partial class ProfileView : UserControl
    {
        public ProfileView(object user)
        {
            InitializeComponent();

            if (user is Student s)
            {
                DataContext = new ProfileViewModel(s);
            }
            else if (user is Tutor t)
            {
                DataContext = new ProfileViewModel(t);
            }
        }

        private void SearchTutor_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentSessionService.CurrentUser is Student s)
            {
                MainView.Instance.NavigateTo(new TutorListView(s));
            }
        }
    }
}