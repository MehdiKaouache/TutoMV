using Projet_Session_Entreprise.Models;
using System.Threading.Tasks;

namespace Projet_Session_Entreprise.Repositories.Interfaces
{
    public interface IStudentRepository : IRepository<Student>
    {
        Task<Student?> GetByDAAsync(string da);
    }
}