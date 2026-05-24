using Projet_Session_Entreprise.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Projet_Session_Entreprise.Core.Interfaces
{
    public interface IAppointmentRepository : IRepository<Appointment>
    {
        Task<IEnumerable<Appointment>> GetByStudentIdAsync(int studentId);
        Task<IEnumerable<Appointment>> GetByTutorIdAsync(int tutorId);
    }
}