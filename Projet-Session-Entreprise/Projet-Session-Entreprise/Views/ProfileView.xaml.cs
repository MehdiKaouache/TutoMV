using System.Windows;
using System.Windows.Controls;
using Projet_Session_Entreprise.Core.Models;
using Projet_Session_Entreprise.UI.ViewModels;

namespace Projet_Session_Entreprise.UI.Views
{
    public partial class ProfileView : UserControl
    {
        private Student? _currentStudent;

        public ProfileView(object user)
        {
            InitializeComponent();

            if (user is Student s)
            {
                _currentStudent = s;
                this.DataContext = new ProfileViewModel(s);
            }
            else if (user is Tutor t)
            {
                this.DataContext = new ProfileViewModel(t);

                if (ctrlAvailability != null)
                {
                    ctrlAvailability.Initialize(t);
                }
            }
        }

        private void SearchTutor_Click(object sender, RoutedEventArgs e)
        {
            if (_currentStudent != null)
            {
                MainView.Instance.NavigateTo(new TutorListView(_currentStudent));
            }
        }
    }
}