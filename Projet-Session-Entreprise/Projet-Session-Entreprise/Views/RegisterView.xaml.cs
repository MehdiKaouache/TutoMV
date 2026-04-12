using Projet_Session_Entreprise.Services;
using Projet_Session_Entreprise.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace Projet_Session_Entreprise.Views
{
    public partial class RegisterView : Window
    {
        public RegisterView()
        {
            InitializeComponent();
            var viewModel = new RegisterViewModel(new AuthService());
            DataContext = viewModel;

            //les password box databind pas bien so
            //s = sender, e = event
            txtPassword.PasswordChanged += (s, e) => viewModel.MotDePasse = txtPassword.Password;
        }

        //le ViewModel devrait s'occuper du click
    }
}
