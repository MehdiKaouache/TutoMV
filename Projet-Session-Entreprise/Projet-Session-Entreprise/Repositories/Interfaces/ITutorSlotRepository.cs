using Projet_Session_Entreprise.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Projet_Session_Entreprise.Repositories.Interfaces
{
    public interface ITutorSlotRepository : IRepository<TutorSlot>
    {
        Task<IEnumerable<TutorSlot>> GetByTutorIdAsync(int tutorId);
    }
}