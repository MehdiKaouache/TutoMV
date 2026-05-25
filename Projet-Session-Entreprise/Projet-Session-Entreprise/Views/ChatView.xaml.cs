using System.Windows.Controls;
using Projet_Session_Entreprise.UI.ViewModels;

namespace Projet_Session_Entreprise.UI.Views
{
    public partial class ChatView : UserControl
    {
        public ChatView()
        {
            InitializeComponent();
            this.DataContextChanged += (s, e) =>
            {
                if (this.DataContext is ChatViewModel vm)
                {
                    vm.Messages.CollectionChanged += (s2, e2) =>
                    {
                        if (e2.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
                        {
                            ChatScrollViewer.ScrollToEnd();
                        }
                    };
                }
            };
        }
    }
}