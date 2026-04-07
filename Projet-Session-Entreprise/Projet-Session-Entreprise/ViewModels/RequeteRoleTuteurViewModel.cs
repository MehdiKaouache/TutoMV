using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Projet_Session_Entreprise.Services;
using System.Threading.Tasks;

namespace Projet_Session_Entreprise.ViewModels
{
    public partial class RequeteRoleTuteurViewModel : ObservableObject
    {
        private readonly TutorService _tutorService;
        private readonly Tutor _tutor;

        [ObservableProperty] private string _statusMessage = string.Empty;
        [ObservableProperty] private string _selectedCourse = string.Empty;
        [ObservableProperty] private double _enteredGrade;

        public RequeteRoleTuteurViewModel(TutorService tutorService, Tutor tutor)
        {
            _tutorService = tutorService;
            _tutor = tutor;
        }

        [RelayCommand]
        private async Task SendRequestAsync()
        {
            if (string.IsNullOrEmpty(SelectedCourse))
            {
                StatusMessage = "Entrez un cours.";
                return;
            }

            bool success = await _tutorService.PromoteTutorAsync(_tutor.Id, EnteredGrade);

            if (success)
            {
                StatusMessage = "Demande acceptée !";
            }
            else
            {
                StatusMessage = "Moyenne insuffisante.";
            }
        }
    }
}