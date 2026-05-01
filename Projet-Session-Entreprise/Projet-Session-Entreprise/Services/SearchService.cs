using Microsoft.EntityFrameworkCore;
using Projet_Session_Entreprise.Models;

namespace Projet_Session_Entreprise.Services
{
    public class SearchService
    {
        private readonly AppDbContext _db;

        public SearchService(AppDbContext db) => _db = db;

        public async Task<List<Tutor>> SearchByNameAsync(string nom)
        {
            return await _db.Tutors
                .Where(t => t.IsValidated &&
                       (t.Nom.ToLower().Contains(nom.ToLower()) || t.Prenom.ToLower().Contains(nom.ToLower())))
                .ToListAsync();
        }

        public async Task<List<Tutor>> SearchBySubjectAsync(string matiere)
        {
            return await _db.Tutors
                .Where(t => t.IsValidated && t.Subject.ToLower().Contains(matiere.ToLower()))
                .ToListAsync();
        }
    }
}