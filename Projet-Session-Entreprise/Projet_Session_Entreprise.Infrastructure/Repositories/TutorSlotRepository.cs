using Microsoft.EntityFrameworkCore;
using Projet_Session_Entreprise.Infrastructure.Data;
using Projet_Session_Entreprise.Core.Models;
using Projet_Session_Entreprise.Core.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Projet_Session_Entreprise.Infrastructure.Repositories
{
    public class TutorSlotRepository : ITutorSlotRepository
    {
        private readonly AppDbContext _context;

        public TutorSlotRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<TutorSlot?> GetByIdAsync(int id) => await _context.TutorSlots.FindAsync(id);

        public async Task<IEnumerable<TutorSlot>> GetAllAsync() => await _context.TutorSlots.ToListAsync();

        public async Task<IEnumerable<TutorSlot>> GetByTutorIdAsync(int tutorId)
        {
            return await _context.TutorSlots
                .Where(s => s.TutorId == tutorId)
                .ToListAsync();
        }

        public async Task AddAsync(TutorSlot entity) => await _context.TutorSlots.AddAsync(entity);

        public void Update(TutorSlot entity) => _context.TutorSlots.Update(entity);

        public void Delete(TutorSlot entity) => _context.TutorSlots.Remove(entity);

        public async Task SaveChangesAsync() => await _context.SaveChangesAsync();
    }
}