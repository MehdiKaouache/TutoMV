using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Threading.Tasks;

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
        [ObservableProperty] private List<Avis> _reviewList;


        public ProfileViewModel(Student student)
        {
            _student = student;
            _isTutor = false;
            DA = student.DA;
            Nom = student.Nom;
            Prenom = student.Prenom;
            ReviewList = new List<Avis>();
        }

        public ProfileViewModel(Tutor tutor)
        {
            _tutor = tutor;
            _isTutor = true;
            DA = tutor.DA;
            Nom = tutor.Nom;
            Prenom = tutor.Prenom;
            Availability = tutor.Availability;
            ReviewList = new List<Avis>() { 
                new Avis {NomEleve = "John Doe", Note = 5, Commentaire = "Excellent tuteur!" }
            }; //review en template vu que y a pas encore de sessions de tutoring
        }

        [RelayCommand]
        private async Task SauvegarderAsync()
        {
            using (var db = new AppDbContext())
            {
                if (_student != null)
                {
                    var s = await db.Students.FindAsync(_student.Id);
                    if (s != null) { s.Nom = Nom; s.Prenom = Prenom; }
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