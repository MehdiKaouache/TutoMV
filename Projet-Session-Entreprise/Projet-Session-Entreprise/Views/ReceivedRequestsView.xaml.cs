using System.Windows.Controls;
using Projet_Session_Entreprise.Models;

namespace Projet_Session_Entreprise.Views
{
    public partial class ReceivedRequestsView : UserControl
    {
        public ReceivedRequestsView(Tutor tutor)
        {
            InitializeComponent();
            this.DataContext = new ViewModels.ReceivedRequestViewModel(tutor);
        }
    }
}