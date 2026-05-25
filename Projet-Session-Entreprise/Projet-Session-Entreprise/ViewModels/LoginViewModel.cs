using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Projet_Session_Entreprise.Infrastructure.Services;
using Projet_Session_Entreprise.Core.Interfaces;
using System.Threading.Tasks;

namespace Projet_Session_Entreprise.UI.ViewModels
{
    public partial class LoginViewModel : ObservableObject
    {
        private readonly IAuthService _authService;

        [ObservableProperty] private string _noDa = string.Empty;
        [ObservableProperty] private string _motDePasse = string.Empty;
        [ObservableProperty] private string _statusMessage = string.Empty;

        public LoginViewModel(IAuthService authService) => _authService = authService;

        [RelayCommand]
        public async Task SendRequestAsync()
        {
            if (string.IsNullOrEmpty(NoDa) || string.IsNullOrEmpty(MotDePasse))
            {
                StatusMessage = "Veuillez remplir tous les champs.";
                return;
            }

            var user = await _authService.LoginAsync(NoDa, MotDePasse);
            if (user != null)
            {
                CurrentSessionService.CurrentUser = user;
                StatusMessage = "Connexion réussie";
            }
            else StatusMessage = "Identifiants invalides";
        }
    }
}