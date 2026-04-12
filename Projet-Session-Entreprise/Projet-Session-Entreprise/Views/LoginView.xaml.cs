using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Projet_Session_Entreprise.Services;
using Projet_Session_Entreprise.ViewModels;
using Projet_Session_Entreprise.Views;

namespace Projet_Session_Entreprise
{
    public partial class LoginView : Window
    {
        public LoginView()
        {
            InitializeComponent();
            var viewModel = new LoginViewModel(new AuthService());
            DataContext = viewModel;
            txtPassword.PasswordChanged += (s, e) => viewModel.MotDePasse = txtPassword.Password;
        }

        //la logique de ce btn devrait être géré par le ViewModel

        private void btnReg_Click(object sender, RoutedEventArgs e)
        {
            var reg = new RegisterView();
            reg.Show();
            this.Close();
        }
    }
}