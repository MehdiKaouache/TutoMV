using Projet_Session_Entreprise.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Projet_Session_Entreprise.Core.Interfaces
{
    public interface ITutorRepository : IRepository<Tutor>
    {
        Task<Tutor?> GetByDAAsync(string da);
        Task<IEnumerable<Tutor>> GetBySubjectAsync(string subject);
        Task<IEnumerable<Tutor>> SearchTutorsAsync(string query);
    }
}