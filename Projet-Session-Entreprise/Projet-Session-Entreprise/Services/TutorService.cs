using Projet_Session_Entreprise.Repositories.Interfaces;
using Projet_Session_Entreprise.Services.Interfaces;
using System.Threading.Tasks;

namespace Projet_Session_Entreprise.Services
{
    public class TutorService : ITutorService
    {
        private readonly ITutorRepository _tutorRepo;

        public TutorService(ITutorRepository tutorRepo)
        {
            _tutorRepo = tutorRepo;
        }

        public async Task<bool> PromoteTutorAsync(int tutorId, double grade)
        {
            var tutor = await _tutorRepo.GetByIdAsync(tutorId);
            if (tutor == null) return false;

            tutor.IsValidated = grade >= 80;
            _tutorRepo.Update(tutor);
            await _tutorRepo.SaveChangesAsync();
            return tutor.IsValidated;
        }
    }
}