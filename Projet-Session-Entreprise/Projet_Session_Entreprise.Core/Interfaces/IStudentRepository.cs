using Projet_Session_Entreprise.Core.Models;
using System.Threading.Tasks;

namespace Projet_Session_Entreprise.Core.Interfaces
{
    public interface IStudentRepository : IRepository<Student>
    {
        Task<Student?> GetByDAAsync(string da);
    }
}