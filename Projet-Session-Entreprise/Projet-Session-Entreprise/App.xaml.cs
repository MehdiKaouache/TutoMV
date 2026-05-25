using Projet_Session_Entreprise.Infrastructure.Data;
using Projet_Session_Entreprise.Infrastructure.Repositories;
using Projet_Session_Entreprise.Core.Interfaces;
using Projet_Session_Entreprise.Infrastructure.Services;
using System;
using System.Windows;

namespace Projet_Session_Entreprise.UI
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
        public static IMessageRepository MessageRepo { get; private set; } = null!;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var context = new AppDbContext();

            TutorRepo = new TutorRepository(context);
            StudentRepo = new StudentRepository(context);
            AppointmentRepo = new AppointmentRepository(context);
            ReviewRepo = new ReviewRepository(context);
            MessageRepo = new MessageRepository(context);

            AuthService = new AuthService(StudentRepo, TutorRepo);
            TutorService = new TutorService(TutorRepo);
            NotificationService = new NotificationService(AppointmentRepo, TutorRepo);
        }
    }
}