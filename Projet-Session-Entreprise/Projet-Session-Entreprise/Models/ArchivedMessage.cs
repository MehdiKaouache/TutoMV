using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projet_Session_Entreprise.Models
{
    public class ArchivedAppointment
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public int TutorId { get; set; }
        public DateTime DateRDV { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime ArchivedAt { get; set; } = DateTime.Now;
    }
}
