using Projet_Session_Entreprise.Data;
using Projet_Session_Entreprise.Repositories;
using Projet_Session_Entreprise.Repositories.Interfaces;
using Projet_Session_Entreprise.Services;
using Projet_Session_Entreprise.Services.Interfaces;
using System;
using System.Windows;

namespace Projet_Session_Entreprise
{
    public partial class App : Application
    {
        public static IAuthService AuthService { get; private set; } = null!;
        public static ITutorService TutorService { get; private set; } = null!;
        public static NotificationService NotificationService { get; private set; } = null!;
        public static ITutorRepository TutorRepo { get; private set; } = null!;
        public static IStudentRepository StudentRepo { get; private set; } = null!;
        public static IAppointmentRepository AppointmentRepo { get; private set; } = null!;
        public static IReviewRepository ReviewRepo { get; private set; } = null!;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var context = new AppDbContext();

            TutorRepo = new TutorRepository(context);
            StudentRepo = new StudentRepository(context);
            AppointmentRepo = new AppointmentRepository(context);
            ReviewRepo = new ReviewRepository(context);

            AuthService = new AuthService(StudentRepo, TutorRepo);
            TutorService = new TutorService(TutorRepo);
            NotificationService = new NotificationService(AppointmentRepo, TutorRepo);
        }
    }
}