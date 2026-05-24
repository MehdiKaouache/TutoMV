using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Projet_Session_Entreprise.Core.Models;
using Projet_Session_Entreprise.Core.Interfaces;

namespace Projet_Session_Entreprise.Infrastructure.Services
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

        public async Task<string> GetAcceptedAppointmentsMessageAsync(Student student)
        {
            if (student == null) return string.Empty;

            try
            {
                var appointments = await _appointmentRepo.GetByStudentIdAsync(student.Id);

                if (appointments == null || !appointments.Any()) return string.Empty;

                var acceptedStatuses = new[] { "Accepté", "Acceptée", "Accepted", "Approuvé" };

                var accepted = appointments.Where(a => a.Status != null && acceptedStatuses.Contains(a.Status)).ToList();

                if (!accepted.Any()) return string.Empty;

                var sb = new StringBuilder();
                sb.AppendLine("Vous avez des rendez-vous acceptés :");

                foreach (var a in accepted)
                {
                    var tutor = await _tutorRepo.GetByIdAsync(a.TutorId);
                    string tutorName = tutor != null ? $"{tutor.Nom} {tutor.Prenom}" : "Enseignant inconnu";
                    sb.AppendLine($"- {a.DateRDV:g} avec {tutorName}");
                }

                return sb.ToString();
            }
            catch
            {
                return string.Empty;
            }
        }
    }
}