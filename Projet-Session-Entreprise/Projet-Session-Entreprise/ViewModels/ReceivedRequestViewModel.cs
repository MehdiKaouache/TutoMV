using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Projet_Session_Entreprise.Core.Models;
using Projet_Session_Entreprise.Infrastructure.Data;
using System.Collections.ObjectModel;
using System.Linq;

namespace Projet_Session_Entreprise.UI.ViewModels
{
    public partial class ReceivedRequestViewModel : ObservableObject
    {
        private readonly Tutor _tutor;
        public ObservableCollection<Appointment> Appointments { get; set; } = new();

        [ObservableProperty] private string _statusMessage = "";
        [ObservableProperty] private bool _isModalOpen;
        [ObservableProperty] private string _modalMessage = "";

        private Appointment? _appointmentToProcess;
        private string _pendingAction = "";

        public ReceivedRequestViewModel(Tutor tutor)
        {
            _tutor = tutor;
            LoadAppointments();
        }

        private void LoadAppointments()
        {
            Appointments.Clear();
            using (var db = new AppDbContext())
            {
                var requests = db.Appointments.Where(a => a.TutorId == _tutor.Id).ToList();
                foreach (var req in requests)
                {
                    Appointments.Add(req);
                }
            }
        }

        [RelayCommand]
        private void PromptAccept(Appointment appt)
        {
            _appointmentToProcess = appt;
            _pendingAction = "Accepté";
            ModalMessage = $"Êtes-vous sûr de vouloir accepter ce rendez-vous du {appt.DateRDV:dd/MM/yyyy HH:mm} ?";
            IsModalOpen = true;
        }

        [RelayCommand]
        private void PromptRefuse(Appointment appt)
        {
            _appointmentToProcess = appt;
            _pendingAction = "Refusé";
            ModalMessage = $"Êtes-vous sûr de vouloir refuser ce rendez-vous du {appt.DateRDV:dd/MM/yyyy HH:mm} ?";
            IsModalOpen = true;
        }

        [RelayCommand]
        private void ConfirmAction()
        {
            if (_appointmentToProcess != null && !string.IsNullOrEmpty(_pendingAction))
            {
                using (var db = new AppDbContext())
                {
                    var existing = db.Appointments.Find(_appointmentToProcess.Id);
                    if (existing != null)
                    {
                        existing.Status = _pendingAction;
                        db.SaveChanges();
                    }
                }
                StatusMessage = _pendingAction == "Accepté" ? "Le rendez-vous a été accepté." : "Le rendez-vous a été refusé.";
                LoadAppointments();
            }
            IsModalOpen = false;
            _appointmentToProcess = null;
        }

        [RelayCommand]
        private void CancelAction()
        {
            IsModalOpen = false;
            _appointmentToProcess = null;
        }
    }
}