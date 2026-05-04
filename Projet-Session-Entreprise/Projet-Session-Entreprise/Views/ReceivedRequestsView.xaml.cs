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
using Projet_Session_Entreprise.Models;

namespace Projet_Session_Entreprise.Views
{
    public partial class ReceivedRequestsView : Window
    {
        public ReceivedRequestsView(Tutor tutor)
        {
            InitializeComponent();
            DataContext = new ViewModels.ReceivedRequestViewModel(tutor);
        }
    }
}
