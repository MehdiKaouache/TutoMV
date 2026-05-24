using Microsoft.EntityFrameworkCore;
using Projet_Session_Entreprise.Core.Models;
using Projet_Session_Entreprise.Infrastructure.Data;
using Projet_Session_Entreprise.Core.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Projet_Session_Entreprise.Infrastructure.Repositories
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly AppDbContext _context;

        public ReviewRepository(AppDbContext context) => _context = context;

        public async Task<Review?> GetByIdAsync(int id) => await _context.Reviews.FindAsync(id);
        public async Task<IEnumerable<Review>> GetAllAsync() => await _context.Reviews.ToListAsync();
        public async Task<IEnumerable<Review>> GetByTutorIdAsync(int tutorId) => await _context.Reviews.Where(r => r.TutorId == tutorId).ToListAsync();
        public async Task AddAsync(Review entity) => await _context.Reviews.AddAsync(entity);
        public void Update(Review entity) => _context.Reviews.Update(entity);
        public void Delete(Review entity) => _context.Reviews.Remove(entity);
        public async Task SaveChangesAsync() => await _context.SaveChangesAsync();
    }
}