using Projet_Session_Entreprise.Core.Models;

namespace Projet_Session_Entreprise.Infrastructure.Services
{
    public class CurrentSessionService
    {
        public static object? CurrentUser { get; set; }
        public static bool IsTutor => CurrentUser is Tutor;
        public static bool IsStudent => CurrentUser is Student;
        public static bool IsConnected => CurrentUser != null;
        public static bool HasSeenNotifications { get; set; } = false;

        public static void ResetSession()
        {
            CurrentUser = null;
            HasSeenNotifications = false;
        }
    }
}