using Microsoft.EntityFrameworkCore;

namespace Projet_Session_Entreprise.Services
{
    public class AuthService
    {
        public async Task<bool> LoginAsync(string da, string password)
        {
            using (var db = new AppDbContext())
            {
                var user = await db.Users
                    .FirstOrDefaultAsync(u => u.DA == da && u.Password == password);

                return user != null;
            }
        }
    }
}