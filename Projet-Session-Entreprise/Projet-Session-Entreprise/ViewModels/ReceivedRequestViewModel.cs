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
            var requests = await App.AppointmentRepo.GetByTutorIdAsync(_tutor.Id);

            if (requests != null)
            {
                foreach (var req in requests.Where(a => a.Status == "En attente"))
                {
                    Appointments.Add(req);
                }
            }
        }

        [RelayCommand]
        private async Task Accepter(Appointment appointment)
        {
            appointment.Status = "Accepté";
            App.AppointmentRepo.Update(appointment);
            await App.AppointmentRepo.SaveChangesAsync();
            Appointments.Remove(appointment);
            MessageBox.Show("Rendez-vous accepté !");
        }

        [RelayCommand]
        private async Task Refuser(Appointment appointment)
        {
            appointment.Status = "Refusé";
            App.AppointmentRepo.Update(appointment);
            await App.AppointmentRepo.SaveChangesAsync();
            Appointments.Remove(appointment);
            MessageBox.Show("Rendez-vous refusé.");
        }
    }
}