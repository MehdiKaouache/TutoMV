using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Projet_Session_Entreprise.UI.ViewModels
{
    public partial class RegisterViewModel : ObservableObject
    {
        [ObservableProperty] private string _nom = string.Empty;
        [ObservableProperty] private string _prenom = string.Empty;
        [ObservableProperty] private string _noDa = string.Empty;
        [ObservableProperty] private string _motDePasse = string.Empty;
        [ObservableProperty] private double _gpa;
        [ObservableProperty] private string _roleSelectionne = "Etudiant";
        [ObservableProperty] private string _statusMessage = string.Empty;

        public List<string> Roles { get; } = new List<string> { "Etudiant", "Enseignant" };

        [RelayCommand]
        private async Task RegisterAsync()
        {
            if (string.IsNullOrEmpty(Nom) || string.IsNullOrEmpty(Prenom) || string.IsNullOrEmpty(NoDa))
            {
                StatusMessage = "Veuillez remplir tous les champs.";
                return;
            }

            bool success = await App.AuthService.RegisterAsync(Nom, Prenom, NoDa, RoleSelectionne, MotDePasse, Gpa);

            if (success)
                StatusMessage = "Compte créé avec succès !";
            else
                StatusMessage = "Erreur : Ce DA est déjà utilisé.";
        }
    }
}