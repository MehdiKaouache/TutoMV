using Projet_Session_Entreprise.ViewModels;
using System.Windows;

namespace Projet_Session_Entreprise.Views
{
    public partial class ProfileView : Window
    {
        public ProfileView()
        {
            InitializeComponent();
            var fakeUser = new AppUser { Id = 1, DA = "123456", Nom = "Tremblay", Prenom = "Hamza", Role = "Étudiant" };
            DataContext = new ProfileViewModel(fakeUser);
        }

        public ProfileView(AppUser user)
        {
            InitializeComponent();
            DataContext = new ProfileViewModel(user);
        }
    }
}
