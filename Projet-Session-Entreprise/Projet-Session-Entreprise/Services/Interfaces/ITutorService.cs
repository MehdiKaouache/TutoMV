using System.Threading.Tasks;

namespace Projet_Session_Entreprise.Services.Interfaces
{
    public interface ITutorService
    {
        Task<bool> PromoteTutorAsync(int tutorId, double grade);
    }
}