using Projet_Session_Entreprise.Models;
using Projet_Session_Entreprise.Repositories.Interfaces;
using Projet_Session_Entreprise.Services.Interfaces;
using System.Threading.Tasks;

namespace Projet_Session_Entreprise.Services
{
    public class AuthService : IAuthService
    {
        private readonly IStudentRepository _studentRepo;
        private readonly ITutorRepository _tutorRepo;

        public AuthService(IStudentRepository studentRepo, ITutorRepository tutorRepo)
        {
            _studentRepo = studentRepo;
            _tutorRepo = tutorRepo;
        }

        private bool MotDePasseValide(string password, string hash)
        {
            try
            {
                return BCrypt.Net.BCrypt.Verify(password, hash);
            }
            catch (BCrypt.Net.SaltParseException)
            {
                return false;
            }
        }

        public async Task<User?> LoginAsync(string da, string password)
        {
            var student = await _studentRepo.GetByDAAsync(da);
            if (student != null && MotDePasseValide(password, student.Password))
                return student;

            var tutor = await _tutorRepo.GetByDAAsync(da);
            if (tutor != null && MotDePasseValide(password, tutor.Password))
                return tutor;

            return null;
        }

        public async Task<bool> RegisterAsync(string nom, string prenom, string da, string role, string password, double gpa)
        {
            var exists = await _studentRepo.GetByDAAsync(da) != null || await _tutorRepo.GetByDAAsync(da) != null;
            if (exists) return false;

            string motDePasseHashe = BCrypt.Net.BCrypt.HashPassword(password);

            if (role == "Etudiant")
            {
                await _studentRepo.AddAsync(new Student
                {
                    Nom = nom,
                    Prenom = prenom,
                    DA = da,
                    Password = motDePasseHashe,
                    Role = "Étudiant",
                    GPA = gpa
                });
            }
            else
            {
                await _tutorRepo.AddAsync(new Tutor
                {
                    Nom = nom,
                    Prenom = prenom,
                    DA = da,
                    Password = motDePasseHashe,
                    Role = "Tuteur",
                    AverageGrade = gpa,
                    Subject = "À définir",
                    Availability = "À définir",
                    IsValidated = false
                });
            }

            await _studentRepo.SaveChangesAsync();
            return true;
        }
    }
}