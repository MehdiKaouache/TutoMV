using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Linq;

namespace Projet_Session_Entreprise.ViewModels
{
    public partial class ProfileViewModel : ObservableObject
    {
        private readonly Student _student;
        private readonly Tutor _tutor;

        [ObservableProperty] private string _dA;
        [ObservableProperty] private string _nom;
        [ObservableProperty] private string _prenom;
        [ObservableProperty] private string _availability;
        [ObservableProperty] private string _statusMessage;
        [ObservableProperty] private bool _isTutor;

        public bool IsStudent => !IsTutor;

        public ObservableCollection<Review> Reviews { get; set; } = new ObservableCollection<Review>();

        public ProfileViewModel(Student student)
        {
            _student = student;
            IsTutor = false;
            DA = student.DA;
            Nom = student.Nom;
            Prenom = student.Prenom;
        }

        public ProfileViewModel(Tutor tutor)
        {
            _tutor = tutor;
            IsTutor = true;
            DA = tutor.DA;
            Nom = tutor.Nom;
            Prenom = tutor.Prenom;
            Availability = tutor.Availability;

            LoadReviews(tutor.Id);
        }

        partial void OnIsTutorChanged(bool value)
        {
            OnPropertyChanged(nameof(IsStudent));
        }

        private void LoadReviews(int tutorId)
        {
            using (var db = new AppDbContext())
            {
                var tutor = db.Tutors
                    .Include(t => t.Reviews)
                    .FirstOrDefault(t => t.Id == tutorId);

                Reviews.Clear();

                if (tutor != null && tutor.Reviews != null)
                {
                    foreach (var review in tutor.Reviews)
                    {
                        Reviews.Add(review);
                    }
                }
            }
        }

        [RelayCommand]
        private async Task SauvegarderAsync()
        {
            using (var db = new AppDbContext())
            {
                if (_student != null)
                {
                    var s = await db.Students.FindAsync(_student.Id);
                    if (s != null)
                    {
                        s.Nom = Nom;
                        s.Prenom = Prenom;
                    }
                }
                else if (_tutor != null)
                {
                    var t = await db.Tutors.FindAsync(_tutor.Id);
                    if (t != null)
                    {
                        t.Nom = Nom;
                        t.Prenom = Prenom;
                        t.Availability = Availability;
                    }
                }

                await db.SaveChangesAsync();
                StatusMessage = "Profil mis à jour !";
            }
        }
    }
}