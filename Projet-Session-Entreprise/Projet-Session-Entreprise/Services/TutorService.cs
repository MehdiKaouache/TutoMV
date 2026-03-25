using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Projet_Session_Entreprise.Services
{
    public class TutorService
    {
        private readonly AppDbContext _db;
        public TutorService(AppDbContext db) => _db = db;

        public async Task<bool> PromoteStudentAsync(int userId, bool signed)
        {
            var user = await _db.Users.FindAsync(userId);
            if (user != null && user.AverageGrade >= 80 && signed)
            {
                user.IsTutor = true;
                user.HasSignedEngagement = true;
                await _db.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}