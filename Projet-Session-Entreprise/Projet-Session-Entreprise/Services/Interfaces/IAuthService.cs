using Projet_Session_Entreprise.Models;
using System.Threading.Tasks;

namespace Projet_Session_Entreprise.Services.Interfaces
{
    public interface IAuthService
    {
        Task<User?> LoginAsync(string da, string password);
        Task<bool> RegisterAsync(string nom, string prenom, string da, string role, string password, double gpa);
    }
}