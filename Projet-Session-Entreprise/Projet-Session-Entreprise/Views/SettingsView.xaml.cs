using System.Windows;
using System.Windows.Controls;
using Projet_Session_Entreprise.UI.ViewModels;

namespace Projet_Session_Entreprise.UI.Views
{
    public partial class SettingsView : UserControl
    {
        private SettingsViewModel _viewModel;

        public SettingsView()
        {
            InitializeComponent();
            _viewModel = new SettingsViewModel();
            this.DataContext = _viewModel;
        }

        private async void BtnUpdateDA_Click(object sender, RoutedEventArgs e)
        {
            await _viewModel.UpdateDAAsync(txtNouveauDA.Text);
        }

        private async void BtnUpdatePassword_Click(object sender, RoutedEventArgs e)
        {
            await _viewModel.UpdatePasswordAsync(txtMotDePasseActuel.Password, txtNouveauMotDePasse.Password, txtConfirmMotDePasse.Password);
            txtMotDePasseActuel.Clear();
            txtNouveauMotDePasse.Clear();
            txtConfirmMotDePasse.Clear();
        }
    }
}