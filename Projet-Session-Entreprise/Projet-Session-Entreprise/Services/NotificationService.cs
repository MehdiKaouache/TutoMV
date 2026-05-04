using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using Projet_Session_Entreprise.Models;

namespace Projet_Session_Entreprise.Services
{
    public static class NotificationService
    {
        // Show a MessageBox summarizing accepted appointments for a student
        public static void ShowAcceptedAppointmentsForStudent(Student student, IEnumerable<Appointment> appointments, IEnumerable<Tutor> tutors)
        {
            if (student == null) return;

            var acceptedStatuses = new[] { "Accepté", "Acceptée", "Accepted", "Accept", "Accepte", "Approved", "Approuvé" };

            var accepted = appointments.Where(a => a.StudentId == student.Id && acceptedStatuses.Contains(a.Status)).ToList();
            if (!accepted.Any()) return;

            var sb = new StringBuilder();
            sb.AppendLine("Vous avez des rendez-vous acceptés :");
            foreach (var a in accepted)
            {
                var tutor = tutors.FirstOrDefault(t => t.Id == a.TutorId);
                string tutorName = tutor != null ? $"{tutor.Nom} {tutor.Prenom}" : "Enseignant inconnu";
                sb.AppendLine($"- {a.DateRDV:g} avec {tutorName}");
            }

            MessageBox.Show(sb.ToString(), "Rendez-vous accepté", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}