using Microsoft.EntityFrameworkCore;
using Projet_Session_Entreprise.Core.Models;

namespace Projet_Session_Entreprise.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext() { }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<Student> Students { get; set; }
        public DbSet<Tutor> Tutors { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<TutorSlot> TutorSlots { get; set; }
        public DbSet<CompletedAppointment> CompletedAppointments { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<ArchivedConversation> ArchivedConversations { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (optionsBuilder.IsConfigured) return;

            string connectionString = "Server=127.0.0.1;Port=3306;Database=schooldb;User=root;Password=1234;";
            optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
        }
    }
}
