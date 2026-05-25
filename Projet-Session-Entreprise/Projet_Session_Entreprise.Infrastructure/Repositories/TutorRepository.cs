using Microsoft.EntityFrameworkCore;
using Projet_Session_Entreprise.Infrastructure.Data;
using Projet_Session_Entreprise.Core.Models;
using Projet_Session_Entreprise.Core.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Projet_Session_Entreprise.Infrastructure.Repositories
{
    public class TutorRepository : ITutorRepository
    {
        private readonly AppDbContext _context;

        public TutorRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Tutor?> GetByIdAsync(int id) => await _context.Tutors.FindAsync(id);
        public async Task<IEnumerable<Tutor>> GetAllAsync() => await _context.Tutors.ToListAsync();
        public async Task<Tutor?> GetByDAAsync(string da) => await _context.Tutors.FirstOrDefaultAsync(t => t.DA == da);
        public async Task<IEnumerable<Tutor>> GetBySubjectAsync(string subject) => await _context.Tutors.Where(t => t.Subject == subject).ToListAsync();
        public async Task<IEnumerable<Tutor>> SearchTutorsAsync(string query)
        {
            return await _context.Tutors
                .Where(t => t.Nom.Contains(query) || t.Prenom.Contains(query) || t.Subject.Contains(query))
                .ToListAsync();
        }
        public async Task AddAsync(Tutor entity) => await _context.Tutors.AddAsync(entity);
        public void Update(Tutor entity) => _context.Tutors.Update(entity);
        public void Delete(Tutor entity) => _context.Tutors.Remove(entity);
        public async Task SaveChangesAsync() => await _context.SaveChangesAsync();
    }
}