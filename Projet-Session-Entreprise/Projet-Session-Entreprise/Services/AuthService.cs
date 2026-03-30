using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Projet_Session_Entreprise.Services
{
    public class AuthService
    {
        public async Task<object?> LoginAsync(string da, string password)
        {
            using (var db = new AppDbContext())
            {
                var student = await db.Students.FirstOrDefaultAsync(s => s.DA == da && s.Password == password);
                if (student != null) return student;

                var tutor = await db.Tutors.FirstOrDefaultAsync(t => t.DA == da && t.Password == password);
                if (tutor != null) return tutor;

                return null;
            }
        }
    }
}