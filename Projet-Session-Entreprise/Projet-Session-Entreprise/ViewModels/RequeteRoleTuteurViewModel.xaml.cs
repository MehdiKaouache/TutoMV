using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Projet_Session_Entreprise.Models;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace Projet_Session_Entreprise.ViewModels
{
    public partial class RequeteRoleTuteurViewModel : ObservableObject
    {
        private readonly Tutor _tutor;

        [ObservableProperty] private string _statusMessage = string.Empty;
        [ObservableProperty] private string _selectedCourse = string.Empty;
        [ObservableProperty] private double _enteredGrade;

        public ObservableCollection<string> SelectedSubjects { get; set; } = new ObservableCollection<string>();
        public ObservableCollection<TutorSlot> TempSlots { get; set; } = new ObservableCollection<TutorSlot>();

        public RequeteRoleTuteurViewModel(Tutor tutor)
        {
            _tutor = tutor;
        }

        [RelayCommand]
        private async Task FinaliserDemandeAsync()
        {
            if (string.IsNullOrEmpty(SelectedCourse) || EnteredGrade < 80)
            {
                StatusMessage = "Note insuffisante (min 80) ou cours non spécifié.";
                return;
            }

            bool success = await App.TutorService.PromoteTutorAsync(_tutor.Id, EnteredGrade);

            if (success)
                StatusMessage = "Félicitations, vous êtes maintenant tuteur !";
            else
                StatusMessage = "Erreur lors de la validation.";
        }
    }
}