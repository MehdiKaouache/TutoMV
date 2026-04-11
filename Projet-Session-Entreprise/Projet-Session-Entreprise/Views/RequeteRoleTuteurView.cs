using System.Windows;
using Projet_Session_Entreprise.Services;
using Projet_Session_Entreprise.ViewModels;

namespace Projet_Session_Entreprise.Views
{
    public partial class RequeteRoleTuteurView : Window
    {
        public RequeteRoleTuteurView(Tutor tutor, AppDbContext db)
        {
            InitializeComponent();

            var vm = new RequeteRoleTuteurViewModel(new TutorService(db), tutor);
            DataContext = vm;

            vm.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(vm.StatusMessage) &&
                    vm.StatusMessage == "Demande acceptée !")
                {
                    MessageBox.Show("Promotion réussie !");
                    new ProfileView(tutor).Show();
                    Close();
                }
            };
        }
    }
}