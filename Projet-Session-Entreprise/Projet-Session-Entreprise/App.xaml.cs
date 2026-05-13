using System.Windows;
using Projet_Session_Entreprise.Data;
using Projet_Session_Entreprise.Repositories;
using Projet_Session_Entreprise.Repositories.Interfaces;
using Projet_Session_Entreprise.Services;

namespace Projet_Session_Entreprise
{
    public partial class App : Application
    {
        public static AuthService AuthService { get; private set; } = null!;
        public static TutorService TutorService { get; private set; } = null!;
        public static NotificationService NotificationService { get; private set; } = null!;

        public static ITutorRepository TutorRepo { get; private set; } = null!;
        public static IStudentRepository StudentRepo { get; private set; } = null!;
        public static IAppointmentRepository AppointmentRepo { get; private set; } = null!;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var context = new AppDbContext();

            TutorRepo = new TutorRepository(context);
            StudentRepo = new StudentRepository(context);
            AppointmentRepo = new AppointmentRepository(context);

            AuthService = new AuthService(StudentRepo, TutorRepo);
            TutorService = new TutorService(TutorRepo);
            NotificationService = new NotificationService(AppointmentRepo, TutorRepo);
        }
    }
}