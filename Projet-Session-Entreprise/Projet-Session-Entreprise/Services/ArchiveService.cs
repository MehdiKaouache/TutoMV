using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Projet_Session_Entreprise.Models;

namespace Projet_Session_Entreprise.Services
{
    public class ArchiveService
    {
        public void ArchiveOldAppointments()
        {
            using (var db = new AppDbContext())
            {
                var oldAppointments = db.Appointments
                    .Where(a => a.Status == "Completed")
                    .ToList();

                foreach (var a in oldAppointments)
                {
                    var archived = new ArchivedAppointment
                    {
                        StudentId = a.StudentId,
                        TutorId = a.TutorId,
                        DateRDV = a.DateRDV,
                        Status = a.Status,
                        ArchivedAt = DateTime.Now
                    };

                    db.ArchivedAppointments.Add(archived);
                    db.Appointments.Remove(a);
                }

                db.SaveChanges();
            }
        }
    }
}
