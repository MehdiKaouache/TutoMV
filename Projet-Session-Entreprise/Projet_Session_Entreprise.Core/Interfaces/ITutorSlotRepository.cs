using Projet_Session_Entreprise.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Projet_Session_Entreprise.Core.Interfaces
{
    public interface ITutorSlotRepository : IRepository<TutorSlot>
    {
        Task<IEnumerable<TutorSlot>> GetByTutorIdAsync(int tutorId);
    }
}