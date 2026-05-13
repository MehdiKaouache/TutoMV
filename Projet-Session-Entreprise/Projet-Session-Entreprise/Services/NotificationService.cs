using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Threading.Tasks;
using Projet_Session_Entreprise.Models;
using Projet_Session_Entreprise.Repositories.Interfaces;

namespace Projet_Session_Entreprise.Services
{
    public class NotificationService
    {
        private readonly IAppointmentRepository _appointmentRepo;
        private readonly ITutorRepository _tutorRepo;

        public NotificationService(IAppointmentRepository appointmentRepo, ITutorRepository tutorRepo)
        {
            _appointmentRepo = appointmentRepo;
            _tutorRepo = tutorRepo;
        }

        public async Task CheckAndShowAcceptedAppointmentsAsync(Student student)
        {
            if (student == null) return;

            var appointments = await _appointmentRepo.GetByStudentIdAsync(student.Id);
            var acceptedStatuses = new[] { "Accepté", "Acceptée", "Accepted", "Approuvé" };

            var accepted = appointments.Where(a => acceptedStatuses.Contains(a.Status)).ToList();
            if (!accepted.Any()) return;

            var sb = new StringBuilder();
            sb.AppendLine("Vous avez des rendez-vous acceptés :");

            foreach (var a in accepted)
            {
                var tutor = await _tutorRepo.GetByIdAsync(a.TutorId);
                string tutorName = tutor != null ? $"{tutor.Nom} {tutor.Prenom}" : "Enseignant inconnu";
                sb.AppendLine($"- {a.DateRDV:g} avec {tutorName}");
            }

            MessageBox.Show(sb.ToString(), "Rendez-vous accepté", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}