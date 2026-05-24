using Microsoft.EntityFrameworkCore;
using Projet_Session_Entreprise.Core.Models;
using Projet_Session_Entreprise.Infrastructure.Data;
using System;
using System.Threading.Tasks;

namespace Projet_Session_Entreprise.Infrastructure.Services
{
    public class AppointmentService
    {
        private readonly AppDbContext _db;

        public AppointmentService(AppDbContext db)
        {
            _db = db;
        }

        public async Task<bool> CompleteAppointmentAsync(int appointmentId, int tutorId)
        {
            var appointment = await _db.Appointments
                .FirstOrDefaultAsync(a => a.Id == appointmentId && a.TutorId == tutorId);

            if (appointment == null)
                return false;

            if (appointment.Status != "Accepté")
                return false;

            bool alreadyCompleted = await _db.CompletedAppointments
                .AnyAsync(c => c.AppointmentId == appointmentId);

            if (alreadyCompleted)
                return false;

            appointment.Status = "Terminé";

            var completedAppointment = new CompletedAppointment
            {
                AppointmentId = appointment.Id,
                StudentId = appointment.StudentId,
                TutorId = appointment.TutorId,
                CompletedDate = DateTime.Now
            };

            _db.CompletedAppointments.Add(completedAppointment);
            await _db.SaveChangesAsync();

            return true;
        }

        public async Task<bool> AddReviewAsync(int completedAppointmentId, int studentId, int rating, string comment)
        {
            if (rating < 1 || rating > 5)
                return false;

            var completedAppointment = await _db.CompletedAppointments
                .FirstOrDefaultAsync(c =>
                    c.Id == completedAppointmentId &&
                    c.StudentId == studentId);

            if (completedAppointment == null)
                return false;

            // CORRECTION ICI : On utilise AppointmentId au lieu de CompletedAppointmentId
            bool alreadyReviewed = await _db.Reviews
                .AnyAsync(r =>
                    r.AppointmentId == completedAppointment.AppointmentId &&
                    r.StudentId == studentId);

            if (alreadyReviewed)
                return false;

            var tutor = await _db.Tutors
                .FirstOrDefaultAsync(t => t.Id == completedAppointment.TutorId);

            if (tutor == null)
                return false;

            var review = new Review
            {
                AppointmentId = completedAppointment.AppointmentId,
                TutorId = tutor.Id,
                StudentId = studentId,
                Rating = rating,
                Comment = comment
            };

            _db.Reviews.Add(review);

            tutor.NumberOfRatings += 1;
            tutor.TotalRatings += rating;

            await _db.SaveChangesAsync();

            return true;
        }
    }
}