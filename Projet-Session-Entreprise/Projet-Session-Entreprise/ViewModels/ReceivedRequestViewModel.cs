using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Projet_Session_Entreprise.Models;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Linq;
using System.Windows;

namespace Projet_Session_Entreprise.ViewModels
{
    public partial class ReceivedRequestViewModel : ObservableObject
    {
        private readonly Tutor _tutor;
        public ObservableCollection<Appointment> Appointments { get; set; } = new();

        public ReceivedRequestViewModel(Tutor tutor)
        {
            _tutor = tutor;
            _ = LoadAppointmentsAsync();
        }

        private async Task LoadAppointmentsAsync()
        {
            Appointments.Clear();
        }

        [RelayCommand]
        private void Accepter(Appointment appointment)
        {
            appointment.Status = "Accepté";
            MessageBox.Show("Rendez-vous accepté !");
        }

        [RelayCommand]
        private void Refuser(Appointment appointment)
        {
            appointment.Status = "Refusé";
            MessageBox.Show("Rendez-vous refusé.");
        }
    }
}