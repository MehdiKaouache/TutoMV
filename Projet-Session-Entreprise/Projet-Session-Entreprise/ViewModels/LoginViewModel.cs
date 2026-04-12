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
                StatusMessage = "Vous devez entrer un numéro de demande d'admission valide";
                return;
            } else if (string.IsNullOrEmpty(MotDePasse))
            {
                StatusMessage = "Veuillez entrer le bon mot de passe";
                return;
            }

            CurrentSession.CurrentUser = await _authService.LoginAsync(NoDa, MotDePasse);

            if (CurrentSession.CurrentUser != null)
                StatusMessage = $"Connexion réussie";
            else
                StatusMessage = "Veuillez réessayer";
        }

    }
}
