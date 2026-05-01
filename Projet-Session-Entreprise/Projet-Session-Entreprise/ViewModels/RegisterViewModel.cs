using CommunityToolkit.Mvvm.ComponentModel;
using Projet_Session_Entreprise.Services;

namespace Projet_Session_Entreprise.ViewModels
{
    public partial class RegisterViewModel : ObservableObject
    {
        private readonly AuthService _authService;

        [ObservableProperty] private string _nom = string.Empty;
        [ObservableProperty] private string _prenom = string.Empty;
        [ObservableProperty] private string _noDa = string.Empty;
        [ObservableProperty] private string _roleSelectionne = "Etudiant";
        [ObservableProperty] private string _statusMessage = string.Empty;

        public List<string> Roles { get; } = new List<string> { "Etudiant", "Enseignant" };

        public RegisterViewModel(AuthService authService)
        {
            _authService = authService;
        }
    }
}