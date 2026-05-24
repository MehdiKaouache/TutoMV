using System.Windows.Controls;
using Projet_Session_Entreprise.Core.Models;

namespace Projet_Session_Entreprise.UI.Views
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