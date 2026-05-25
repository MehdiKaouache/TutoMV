using Projet_Session_Entreprise.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Projet_Session_Entreprise.Core.Interfaces
{
    public interface IReviewRepository
    {
        Task<IEnumerable<Review>> GetByTutorIdAsync(int tutorId);
        Task AddAsync(Review review);
        Task SaveChangesAsync();
    }
}