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
    public partial class LoginViewModel : ObservableObject
    {
        private readonly AuthService _authService;
        private readonly AppUser _user;

        [ObservableProperty] private string _noDa = string.Empty;
        [ObservableProperty] private string _motDePasse = string.Empty;
        [ObservableProperty] private string _statusMessage = string.Empty;



        public LoginViewModel(AuthService authService, AppUser user)
        {
            _authService = authService;
            _user = user;
        }

        [RelayCommand]
        private async Task SendRequestAsync()
        {
            if (string.IsNullOrEmpty(NoDa))
            {
                StatusMessage = "Vous devez entrer un numéro de demande d'admission valide";
                return;
            } else if (string.IsNullOrEmpty(MotDePasse))
            {
                StatusMessage = "Veuillez entrer le bon mot de passe";
                return;
            }

            bool success = await _authService.LoginAsync(NoDa, MotDePasse);

            if (success)
                StatusMessage = $"Connexion réussie";
            else
                StatusMessage = "Veuillez réessayer";
        }

    }
}
