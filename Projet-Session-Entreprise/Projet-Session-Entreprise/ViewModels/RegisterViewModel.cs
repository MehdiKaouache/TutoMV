using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Projet_Session_Entreprise.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projet_Session_Entreprise.ViewModels
{
    public partial class RegisterViewModel : ObservableObject
    {
        private readonly AuthService _authService;

        [ObservableProperty] private string _nom = string.Empty;
        [ObservableProperty] private string _prenom = string.Empty;
        [ObservableProperty] private string _noDa = string.Empty;
        [ObservableProperty] private string _roleSelectionne = string.Empty;
        [ObservableProperty] private string _motDePasse = string.Empty;
        [ObservableProperty] private string _statusMessage = string.Empty;

        public List<string> Roles { get; } = new List<string>
        {
            "Etudiant",
            "Enseignant"
        };

        public RegisterViewModel(AuthService authService)
        {
            _authService = authService;
            _roleSelectionne = "Etudiant";
        }

        [RelayCommand]
        private async Task SendRequestAsync()
        {

            if (string.IsNullOrEmpty(Nom) || string.IsNullOrEmpty(Prenom) || string.IsNullOrEmpty(RoleSelectionne) || string.IsNullOrEmpty(NoDa) || string.IsNullOrEmpty(MotDePasse))
            {
                StatusMessage = "Vous devez entrer toutes les informations demandées";
                return;
            }

            bool success = await _authService.LoginAsync(NoDa, MotDePasse);

            if (success)
                StatusMessage = $"Création de compte réussie";
            else
                StatusMessage = "Veuillez réessayer";
        }

    }
}
