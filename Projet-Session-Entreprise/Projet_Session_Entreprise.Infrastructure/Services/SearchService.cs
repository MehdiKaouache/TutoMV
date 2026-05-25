using Projet_Session_Entreprise.Core.Models;
using Projet_Session_Entreprise.Core.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Projet_Session_Entreprise.Infrastructure.Services
{
    public class SearchService
    {
        private readonly ITutorRepository _tutorRepo;

        public SearchService(ITutorRepository tutorRepo)
        {
            _tutorRepo = tutorRepo;
        }

        public async Task<IEnumerable<Tutor>> SearchByNameAsync(string nom)
        {
            var tutors = await _tutorRepo.SearchTutorsAsync(nom);
            return tutors;
        }

        public async Task<IEnumerable<Tutor>> SearchBySubjectAsync(string matiere)
        {
            var tutors = await _tutorRepo.GetBySubjectAsync(matiere);
            return tutors;
        }

        public async Task<IEnumerable<Tutor>> GetAllTutorsAsync()
        {
            return await _tutorRepo.GetAllAsync();
        }
    }
}