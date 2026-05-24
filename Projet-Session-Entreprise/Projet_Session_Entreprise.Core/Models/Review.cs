namespace Projet_Session_Entreprise.Core.Models
{
    public class Review
    {
        public int Id { get; set; }

        public int AppointmentId { get; set; }
        public Appointment? Appointment { get; set; }

        public int TutorId { get; set; }
        public Tutor? Tutor { get; set; }

        public int StudentId { get; set; }
        public Student? Student { get; set; }

        public string? Comment { get; set; }

        public int Rating { get; set; }
    }
}