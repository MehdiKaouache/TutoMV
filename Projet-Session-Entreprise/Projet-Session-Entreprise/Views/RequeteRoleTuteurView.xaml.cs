using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
namespace Projet_Session_Entreprise.Views
{
    public partial class RequeteRoleTuteurView : Window
    {

        public RequeteRoleTuteurView()
        {
            InitializeComponent();
           
            var fakeUser = new AppUser { Id = 1, AverageGrade = 85 };
           
        }

        public RequeteRoleTuteurView(AppUser currentUser, AppDbContext db)
        {
            InitializeComponent();
            var service = new Services.TutorService(db);
            this.DataContext = new ViewModels.RequeteRoleTuteurViewModel(service, currentUser);
        }
    }
}
