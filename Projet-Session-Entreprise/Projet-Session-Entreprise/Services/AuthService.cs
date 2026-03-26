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

        public async Task<bool> RegisterAsync(string nom, 
            string prenom, string da, string role, string password)
        {
            using (var db = new AppDbContext())
            {
                bool compteExiste = await db.Users
                    .AnyAsync(u => u.DA == da);

                if (compteExiste)
                {
                    return false;
                }

                if (role == "Etudiant")
                {
                    var newUser = new AppUser
                    {
                        Nom = nom,
                        Prenom = prenom,
                        DA = da,
                        Password = password,
                    };

                    db.Users.Add(newUser);
                } /* else if (role == "Enseignant") il faut changer la classe teacher et la view 
                                                    register pour les teacher ou bien add un code
                                                    comme les da pour les teachers
                {
                    var newUser = new Teacher
                    {
                        Nom = nom,
                        Prenom = prenom,
                        DA = da,
                        Password = password,
                    };

                    db.Add(newUser);
                } */

                await db.SaveChangesAsync();
                return true;
            }
        }
    }
}