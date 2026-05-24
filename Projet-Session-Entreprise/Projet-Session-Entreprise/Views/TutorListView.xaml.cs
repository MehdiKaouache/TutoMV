using System.Windows.Controls;
using Projet_Session_Entreprise.Core.Models;
using Projet_Session_Entreprise.UI.ViewModels;

namespace Projet_Session_Entreprise.UI.Views
{
    public partial class TutorListView : UserControl
    {
        public TutorListView()
        {
            InitializeComponent();
            this.DataContext = new TutorListViewModel();
        }

        public TutorListView(Student student)
        {
            InitializeComponent();
            this.DataContext = new TutorListViewModel(student);
        }
    }
}