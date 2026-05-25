using System;

namespace Projet_Session_Entreprise.Core.Models
{
    public class Appointment
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public int TutorId { get; set; }
        public DateTime DateRDV { get; set; }
        public string? Status { get; set; }
        public string? Duree { get; set; }
        public string? Lieu { get; set; }
        public Student? Student { get; set; }
        public Tutor? Tutor { get; set; }
    }
}