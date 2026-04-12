using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projet_Session_Entreprise.Services
{
    public class CurrentSession //la session existe seulement si un utilisateur est connecté
    {
        public static object? CurrentUser { get; set; }
        public static bool IsTutor { get
            {
               if (CurrentUser != null && CurrentUser.GetType() == typeof(Tutor))
                {
                    return true;
                }
                return false;
            }
        }
        public static bool IsStudent
        {
            get
            {
                if (CurrentUser != null && CurrentUser.GetType() == typeof(Student))
                {
                    return true;
                }
                return false;
            }
        }
    }
}
