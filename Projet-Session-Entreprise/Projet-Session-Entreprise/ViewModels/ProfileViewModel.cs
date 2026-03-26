using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Threading.Tasks;

namespace Projet_Session_Entreprise.ViewModels
{
    public partial class ProfileViewModel : ObservableObject
    {
        private readonly AppUser _user;

        [ObservableProperty] private string _dA = string.Empty;
        [ObservableProperty] private string _nom = string.Empty;
        [ObservableProperty] private string _prenom = string.Empty;
        [ObservableProperty] private string _role = string.Empty;
        [ObservableProperty] private string _statusMessage = string.Empty;

        public ProfileViewModel(AppUser user)
        {
            _user = user;
            DA = user.DA;
            Nom = user.Nom;
            Prenom = user.Prenom;
            Role = user.Role;
        }

        [RelayCommand]
        private async Task SauvegarderAsync()
        {
            using (var db = new AppDbContext())
            {
                var userInDb = await db.Users.FindAsync(_user.Id);

                if (userInDb == null)
                {
                    StatusMessage = "Utilisateur introuvable.";
                    return;
                }

                userInDb.Nom = Nom;
                userInDb.Prenom = Prenom;

                await db.SaveChangesAsync();
                StatusMessage = "Profil mis à jour avec succès.";
            }
        }
    }
}
