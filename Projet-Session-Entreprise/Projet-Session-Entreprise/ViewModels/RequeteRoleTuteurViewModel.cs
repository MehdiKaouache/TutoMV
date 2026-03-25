using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Projet_Session_Entreprise.Services;
using System.Threading.Tasks;

namespace Projet_Session_Entreprise.ViewModels
{
    public partial class RequeteRoleTuteurViewModel : ObservableObject
    {
        private readonly TutorService _tutorService;
        private readonly AppUser _user;

        [ObservableProperty] private string _statusMessage = string.Empty;
        [ObservableProperty] private string _selectedCourse = string.Empty;


        public RequeteRoleTuteurViewModel(TutorService tutorService, AppUser user)
        {
            _tutorService = tutorService;
            _user = user;
        }

        [RelayCommand]
        private async Task SendRequestAsync()
        {
            if (string.IsNullOrEmpty(SelectedCourse))
            {
                StatusMessage = "Vous devez entrer un nom de cours.";
                return;
            }

            bool success = await _tutorService.PromoteStudentAsync(_user.Id, true);

            if (success)
                StatusMessage = $"Demande envoyée pour le cours : {SelectedCourse}";
            else
                StatusMessage = "Votre moyenne est inférieure à 80%.";
        }
    }
}