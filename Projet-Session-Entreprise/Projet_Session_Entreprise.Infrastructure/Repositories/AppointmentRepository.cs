using Microsoft.EntityFrameworkCore;
using Projet_Session_Entreprise.Infrastructure.Data;
using Projet_Session_Entreprise.Core.Models;
using Projet_Session_Entreprise.Core.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Projet_Session_Entreprise.Infrastructure.Repositories
{
    public class AppointmentRepository : IAppointmentRepository
    {
        private readonly AppDbContext _context;

        public AppointmentRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Appointment?> GetByIdAsync(int id) => await _context.Appointments.FindAsync(id);
        public async Task<IEnumerable<Appointment>> GetAllAsync() => await _context.Appointments.ToListAsync();
        public async Task<IEnumerable<Appointment>> GetByStudentIdAsync(int studentId) => await _context.Appointments.Where(a => a.StudentId == studentId).ToListAsync();
        public async Task<IEnumerable<Appointment>> GetByTutorIdAsync(int tutorId) => await _context.Appointments.Where(a => a.TutorId == tutorId).ToListAsync();
        public async Task AddAsync(Appointment entity) => await _context.Appointments.AddAsync(entity);
        public void Update(Appointment entity) => _context.Appointments.Update(entity);
        public void Delete(Appointment entity) => _context.Appointments.Remove(entity);
        public async Task SaveChangesAsync() => await _context.SaveChangesAsync();
    }
}