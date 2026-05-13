using Projet_Session_Entreprise.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Projet_Session_Entreprise.Repositories.Interfaces
{
    public interface IReviewRepository : IRepository<Review>
    {
        Task<IEnumerable<Review>> GetByTutorIdAsync(int tutorId);
    }
}