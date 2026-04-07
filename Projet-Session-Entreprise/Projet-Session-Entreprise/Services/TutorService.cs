using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Projet_Session_Entreprise.Services
{
    public class TutorService
    {
        private readonly AppDbContext _db;

        public TutorService(AppDbContext db) => _db = db;

        public async Task<bool> PromoteTutorAsync(int tutorId, double gradeSaisie)
        {
            var tutor = await _db.Tutors.FindAsync(tutorId);

            if (tutor != null && gradeSaisie >= 80)
            {
                tutor.AverageGrade = gradeSaisie;
                tutor.IsValidated = true;
                await _db.SaveChangesAsync();
                return true;
            }

            return false;
        }
    }
}