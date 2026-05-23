using System.Windows;
using System.Windows.Controls;
using Projet_Session_Entreprise.Models;
using Projet_Session_Entreprise.ViewModels;

namespace Projet_Session_Entreprise.Views
{
    public partial class ProfileView : UserControl
    {
        private ProfileViewModel? _viewModel;
        private Student? _currentStudent;

        public ProfileView(object user)
        {
            InitializeComponent();

            if (user is Student s)
            {
                _currentStudent = s;
                _viewModel = new ProfileViewModel(s);
                AvailabilityContainer.Visibility = Visibility.Collapsed;
                colDispos.Width = new GridLength(0);
            }
            else if (user is Tutor t)
            {
                _viewModel = new ProfileViewModel(t);
                AvailabilityContainer.Visibility = Visibility.Visible;
                colDispos.Width = new GridLength(350);
                ctrlAvailability.Initialize(t);
            }

            if (_viewModel != null)
            {
                this.DataContext = _viewModel;
                if (this.FindName("dgAppointments") != null)
                {
                    var dg = (DataGrid)this.FindName("dgAppointments");
                    dg.ItemsSource = _viewModel.MyAppointments;
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