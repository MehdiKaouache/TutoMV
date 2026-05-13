using Projet_Session_Entreprise.Models;
using Projet_Session_Entreprise.Repositories.Interfaces;
using System.Threading.Tasks;

namespace Projet_Session_Entreprise.Services
{
    public class TutorService
    {
        private readonly ITutorRepository _tutorRepo;

        public TutorService(ITutorRepository tutorRepo)
        {
            _tutorRepo = tutorRepo;
        }

        public async Task<bool> PromoteTutorAsync(int tutorId, double gradeSaisie)
        {
            var tutor = await _tutorRepo.GetByIdAsync(tutorId);

            if (tutor != null && gradeSaisie >= 80)
            {
                tutor.AverageGrade = gradeSaisie;
                tutor.IsValidated = true;
                _tutorRepo.Update(tutor);
                await _tutorRepo.SaveChangesAsync();
                return true;
            }

            return false;
        }
    }
}