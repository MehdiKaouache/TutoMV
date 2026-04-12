using Projet_Session_Entreprise.Services;
using Projet_Session_Entreprise.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Projet_Session_Entreprise.Views
{
    /// <summary>
    /// Interaction logic for SidebarControlView.xaml
    /// </summary>
    public partial class SidebarControlView : UserControl
    {
        public SidebarControlView()
        {
            InitializeComponent();
        }
        private void btnProfile_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentSession.IsStudent) {
                new ProfileView((Student)CurrentSession.CurrentUser).Show();
            } else if (CurrentSession.IsTutor)
            {
                new ProfileView((Tutor)CurrentSession.CurrentUser).Show();
            } else
            {
                Console.WriteLine("Vous devez être connecté");
            }
        }

        private void btnHome_Click(object sender, RoutedEventArgs e)
        {
            new MainWindow().Show();
        }

        private void btnRegister_Click (object sender, RoutedEventArgs e)
        {
            new RegisterView().Show();
        }

        private void btnPromoteTutor_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentSession.IsTutor)
            {
                new RequeteRoleTuteurView((Tutor)CurrentSession.CurrentUser, new AppDbContext()).Show();
            } else
            {
                Console.WriteLine("Vous devez être tuteur");
            }
        }

        private void btnLogInOrOut (object sender, RoutedEventArgs e)
        {
            if (CurrentSession.CurrentUser != null)
            {
                CurrentSession.CurrentUser = null;
                new MainWindow().Show();
            } else
            {
                new LoginView().Show();
            }
        }
    }
}

