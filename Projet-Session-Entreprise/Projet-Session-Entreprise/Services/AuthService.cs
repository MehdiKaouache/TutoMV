using Microsoft.EntityFrameworkCore;
using Projet_Session_Entreprise.Models;

namespace Projet_Session_Entreprise.Services
{
    public class AuthService
    {
        public async Task<object?> LoginAsync(string da, string password)
        {
            using (var db = new AppDbContext())
            {
                // On cherche par DA seulement, puis on vérifie le mot de passe avec BCrypt
                var student = await db.Students.FirstOrDefaultAsync(s => s.DA == da);
                if (student != null && BCrypt.Net.BCrypt.Verify(password, student.Password))
                    return student;

                var tutor = await db.Tutors.FirstOrDefaultAsync(t => t.DA == da);
                if (tutor != null && BCrypt.Net.BCrypt.Verify(password, tutor.Password))
                    return tutor;

                return null;
            }
        }

        public async Task<bool> RegisterAsync(string nom, string prenom, string da, string role, string password, double gpa)
        {
            using (var db = new AppDbContext())
            {
                bool exists = await db.Students.AnyAsync(u => u.DA == da) || await db.Tutors.AnyAsync(u => u.DA == da);
                if (exists) return false;

                string motDePasseHashe = BCrypt.Net.BCrypt.HashPassword(password);

                if (role == "Etudiant")
                {
                    db.Students.Add(new Student
                    {
                        Nom = nom,
                        Prenom = prenom,
                        DA = da,
                        Password = motDePasseHashe,
                        AverageGrade = gpa,
                        Role = "Étudiant"
                    });
                }
                else
                {
                    db.Tutors.Add(new Tutor
                    {
                        Nom = nom,
                        Prenom = prenom,
                        DA = da,
                        Password = motDePasseHashe,
                        AverageGrade = gpa,
                        Role = "Tuteur",
                        IsValidated = false
                    });
                }

                await db.SaveChangesAsync();
                return true;
            }
        }
    }
}