using Projet_Session_Entreprise.ViewModels;
using System.Windows;

namespace Projet_Session_Entreprise.Views
{
    public partial class SearchView : Window
    {
        public SearchView()
        {
            InitializeComponent();
            DataContext = new SearchViewModel();
        }
    }
}
