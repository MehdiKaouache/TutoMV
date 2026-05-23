using System.Windows.Controls;
using Projet_Session_Entreprise.Models;
using Projet_Session_Entreprise.ViewModels;

namespace Projet_Session_Entreprise.Views
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