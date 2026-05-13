using Projet_Session_Entreprise.Models;
using Projet_Session_Entreprise.Repositories.Interfaces;
using System.Threading.Tasks;

namespace Projet_Session_Entreprise.Services
{
    public class AuthService
    {
        private readonly IStudentRepository _studentRepo;
        private readonly ITutorRepository _tutorRepo;

        public AuthService(IStudentRepository studentRepo, ITutorRepository tutorRepo)
        {
            _studentRepo = studentRepo;
            _tutorRepo = tutorRepo;
        }

        public async Task<object?> LoginAsync(string da, string password)
        {
            var student = await _studentRepo.GetByDAAsync(da);
            if (student != null && student.Password == password) return student;

            var tutor = await _tutorRepo.GetByDAAsync(da);
            if (tutor != null && tutor.Password == password) return tutor;

            return null;
        }

        public async Task<bool> RegisterAsync(string nom, string prenom, string da, string role, string password, double gpa)
        {
            var exists = await _studentRepo.GetByDAAsync(da) != null || await _tutorRepo.GetByDAAsync(da) != null;
            if (exists) return false;

            if (role == "Etudiant")
            {
                await _studentRepo.AddAsync(new Student
                {
                    Nom = nom,
                    Prenom = prenom,
                    DA = da,
                    Password = password,
                    AverageGrade = gpa,
                    Role = "Étudiant"
                });
                await _studentRepo.SaveChangesAsync();
            }
            else
            {
                await _tutorRepo.AddAsync(new Tutor
                {
                    Nom = nom,
                    Prenom = prenom,
                    DA = da,
                    Password = password,
                    AverageGrade = gpa,
                    Role = "Tuteur",
                    IsValidated = false
                });
                await _tutorRepo.SaveChangesAsync();
            }

            return true;
        }
    }
}