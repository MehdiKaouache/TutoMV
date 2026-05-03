using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Projet_Session_Entreprise.Services;
using Projet_Session_Entreprise.Models;
using System.Collections.ObjectModel;

namespace Projet_Session_Entreprise.ViewModels
{
    public partial class RequeteRoleTuteurViewModel : ObservableObject
    {
        private readonly TutorService _tutorService;
        private readonly Tutor _tutor;

        [ObservableProperty] private string _statusMessage = string.Empty;
        [ObservableProperty] private string _selectedCourse = string.Empty;
        [ObservableProperty] private double _enteredGrade;

        public ObservableCollection<string> SelectedSubjects { get; set; } = new ObservableCollection<string>();
        public ObservableCollection<TutorSlot> TempSlots { get; set; } = new ObservableCollection<TutorSlot>();

        public RequeteRoleTuteurViewModel(TutorService tutorService, Tutor tutor)
        {
            _tutorService = tutorService;
            _tutor = tutor;
        }

        [RelayCommand]
        private async Task FinaliserDemandeAsync()
        {
            if (string.IsNullOrEmpty(SelectedCourse) || EnteredGrade < 80)
            {
                StatusMessage = "Note insuffisante ou cours non spécifié.";
                return;
            }

            using (var db = new AppDbContext())
            {
                var t = db.Tutors.Find(_tutor.Id);
                if (t != null)
                {
                    t.Subject = SelectedCourse;
                    t.AverageGrade = EnteredGrade;
                    t.IsValidated = true;

                    foreach (var slot in TempSlots)
                    {
                        slot.TutorId = t.Id;
                        db.TutorSlots.Add(slot);
                    }
                    await db.SaveChangesAsync();
                    StatusMessage = "Félicitations, vous êtes tuteur !";
                }
            }
        }
    }
}