using Projet_Session_Entreprise.Models;

namespace Projet_Session_Entreprise.Services
{
    public class CurrentSessionService
    {
        public static object? CurrentUser { get; set; }
        public static bool IsTutor => CurrentUser is Tutor;
        public static bool IsStudent => CurrentUser is Student;
    }
}