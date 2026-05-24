using System.Windows.Controls;
using Projet_Session_Entreprise.Core.Models;
using Projet_Session_Entreprise.UI.ViewModels;

namespace Projet_Session_Entreprise.UI.Views
{
    public partial class AppointmentsView : UserControl
    {
        public AppointmentsView(object user)
        {
            InitializeComponent();

            if (user is Student s)
            {
                this.DataContext = new ProfileViewModel(s);
            }
            else if (user is Tutor t)
            {
                this.DataContext = new ProfileViewModel(t);
            }
        }
    }
}