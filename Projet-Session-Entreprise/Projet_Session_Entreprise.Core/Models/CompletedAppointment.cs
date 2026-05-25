namespace Projet_Session_Entreprise.Core.Models
{
    public class CompletedAppointment
    {
        public int Id { get; set; }

        public int AppointmentId { get; set; }
        public Appointment? Appointment { get; set; }

        public int StudentId { get; set; }
        public Student? Student { get; set; }

        public int TutorId { get; set; }
        public Tutor? Tutor { get; set; }

        public DateTime CompletedDate { get; set; } = DateTime.Now;
    }
}
