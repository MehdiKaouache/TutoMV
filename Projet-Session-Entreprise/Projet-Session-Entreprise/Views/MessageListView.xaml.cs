using System.Windows.Controls;
using Projet_Session_Entreprise.UI.ViewModels;

namespace Projet_Session_Entreprise.UI.Views
{
    public partial class MessageListView : UserControl
    {
        public MessageListView()
        {
            InitializeComponent();
            this.DataContext = new MessageListViewModel();
        }
    }
}