using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Projet_Session_Entreprise.Models;

namespace Projet_Session_Entreprise.ViewModels
{
    public partial class ReceivedRequestViewModel : ObservableObject
    {
        private readonly Tutor _tutor;

        public ObservableCollection<Appointment> Appointments { get; set; } = new();

        // Notification et chargement
        [ObservableProperty]
        private string _notificationMessage;

        private bool _isLoading;
        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        public ReceivedRequestViewModel(Tutor tutor)
        {
            _tutor = tutor;
            LoadAppointments();
        }

        private void LoadAppointments()
        {
            using (var db = new AppDbContext())
            {
                var list = db.Appointments
                    .Where(a => a.TutorId == _tutor.Id && a.Status == "En attente")
                    .ToList();

                Appointments.Clear();

                foreach (var a in list)
                    Appointments.Add(a);
            }
        }

        [RelayCommand]
        private async Task AccepterAsync(Appointment appointment)
        {
            if (appointment == null) return;

            IsLoading = true;

            using (var db = new AppDbContext())
            {
                var a = await db.Appointments.FindAsync(appointment.Id);
                if (a != null)
                {
                    a.Status = "Accepté";
                    await db.SaveChangesAsync();
                    NotificationMessage = "Rendez-vous accepté !";
                }
            }

            LoadAppointments();
            IsLoading = false;
        }

        [RelayCommand]
        private async Task RefuserAsync(Appointment appointment)
        {
            if (appointment == null) return;

            IsLoading = true;

            using (var db = new AppDbContext())
            {
                var a = await db.Appointments.FindAsync(appointment.Id);
                if (a != null)
                {
                    a.Status = "Refusé";
                    await db.SaveChangesAsync();
                    NotificationMessage = "Rendez-vous refusé !";
                }
            }

            LoadAppointments();
            IsLoading = false;
        }
    }
}