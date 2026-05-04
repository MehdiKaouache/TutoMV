using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Projet_Session_Entreprise.Services;

namespace Projet_Session_Entreprise.ViewModels
{
    public partial class LoginViewModel : ObservableObject
    {
        private readonly AuthService _authService;

        [ObservableProperty] private string _noDa = string.Empty;
        [ObservableProperty] private string _motDePasse = string.Empty;
        [ObservableProperty] private string _statusMessage = string.Empty;

        public LoginViewModel(AuthService authService)
        {
            _authService = authService;
        }

        [RelayCommand]
        private async Task SendRequestAsync()
        {
            if (string.IsNullOrEmpty(NoDa))
            {
                StatusMessage = "Numéro DA requis";
                return;
            }

            if (string.IsNullOrEmpty(MotDePasse))
            {
                StatusMessage = "Mot de passe requis";
                return;
            }

            CurrentSessionService.CurrentUser = await _authService.LoginAsync(NoDa, MotDePasse);

            if (CurrentSessionService.CurrentUser != null)
                StatusMessage = "Connexion réussie";
            else
                StatusMessage = "Identifiants invalides";
        }
    }
}